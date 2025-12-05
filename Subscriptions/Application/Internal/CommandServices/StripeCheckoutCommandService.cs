using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class StripeCheckoutCommandService(IPaymentGatewayService paymentGateway, IPlanRepository planRepository, ISubscriptionRepository subscriptionRepository, ExternalIamService externalIamService, IUnitOfWork unitOfWork, ILogger<StripeCheckoutCommandService> logger) : IStripeCheckoutCommandService
{
    /// <summary>
    /// Creates a Stripe Checkout session to subscribe the user.    
    /// </summary>
    public async Task<string> Handle(CreateCheckoutSessionCommand command)
    {
        logger.LogInformation(
            "Creates a Checkout session for User {UserId} - Plan {PlanId}",
            command.UserId,
            command.PlanId);

        // 1. Validate that the user exists
        var userInfo = await externalIamService.GetUserInfoAsync(command.UserId);
        if (userInfo == null)
            throw new ArgumentException($"User {command.UserId} not found");

        // 2. Validate that the plan exists
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null)
            throw new ArgumentException($"Plan {command.PlanId} not found");

        if (string.IsNullOrEmpty(plan.StripePriceId))
            throw new InvalidOperationException($"Plan {plan.Name} does not have a Stripe Price ID configured");

        // 3. Verifies if the user already has an active subscription
        var existingSubscription = await subscriptionRepository.F(command.UserId);

        if (existingSubscription != null)
            throw new InvalidOperationException(
                $"User {command.UserId} already has an active subscription");

        // 4. Creates a Customer in Stripe
        var stripeCustomerId = await paymentGateway.CreateOrGetCustomerAsync(
            command.UserId,
            userInfo.Email,
            userInfo.FullName);

        // 5. Creates a Checkout session
        var checkoutUrl = await paymentGateway.CreateCheckoutSessionAsync(
            stripeCustomerId,
            new StripePriceId(plan.StripePriceId),
            command.SuccessUrl,
            command.CancelUrl,
            command.TrialPeriodDays);

        logger.LogInformation(
            "Creates a Checkout session for a User {UserId}: {CheckoutUrl}",
            command.UserId,
            checkoutUrl);

        return checkoutUrl;
    }

    /// <summary>
    /// Cancels a subscription in Stripe.
    /// </summary>
    public async Task Handle(CancelSubscriptionInStripeCommand command)
    {
        logger.LogInformation(
            "Cancelling subscription {SubscriptionId} in Stripe (Immediately: {Immediately})",
            command.SubscriptionId,
            command.Immediately);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        var stripeSubscriptionId = new StripeSubscriptionId(subscription.StripeSubscriptionId);

        // 2. Cancels the subscription in Stripe
        if (command.Immediately)
        {
            await paymentGateway.CancelSubscriptionImmediatelyAsync(stripeSubscriptionId);
            subscription.UpdateStatus(ESubscriptionStatus.Cancelled);
        }
        else
        {
            var cancelAt = await paymentGateway.CancelSubscriptionAtPeriodEndAsync(stripeSubscriptionId);
            subscription.ScheduleCancellation(cancelAt);
        }

        // 3. Persist changes
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription {SubscriptionId} cancelled in Stripe",
            command.SubscriptionId);
    }

    /// <summary>
    /// Changes the plan of a subscription in Stripe.
    /// </summary>
    public async Task Handle(ChangeSubscriptionPlanInStripeCommand command)
    {
        logger.LogInformation(
            "Changing subscription plan {SubscriptionId} to Plan {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        // 2. Find the new plan
        var newPlan = await planRepository.FindByIdAsync(new PlanId(command.NewPlanId));
        if (newPlan == null)
            throw new ArgumentException($"Plan {command.NewPlanId} not found");

        if (string.IsNullOrEmpty(newPlan.StripePriceId))
            throw new InvalidOperationException($"Plan {newPlan.Name} does not have a Stripe Price ID");

        // 3. Updates in Stripe
        var stripeSubscriptionId = new StripeSubscriptionId(subscription.StripeSubscriptionId);
        var updatedStripeSubscription = await paymentGateway.UpdateSubscriptionPlanAsync(
            stripeSubscriptionId,
            new StripePriceId(newPlan.StripePriceId),
            command.ProrationBehavior);

        // 4. Update in our DB
        subscription.ChangePlan(
            new PlanId(command.NewPlanId),
            updatedStripeSubscription.CurrentPeriodEnd);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription plan {SubscriptionId} updated to {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);
    }
}