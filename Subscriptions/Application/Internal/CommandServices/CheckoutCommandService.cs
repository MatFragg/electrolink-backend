using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class StripeCheckoutCommandService(IPaymentGatewayService paymentGateway, IPlanRepository planRepository, ISubscriptionRepository subscriptionRepository, ExternalIamService externalIamService,ExternalProfileService externalProfileService, IUnitOfWork unitOfWork, ILogger<StripeCheckoutCommandService> logger) : IStripeCheckoutCommandService
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
        if (!await externalIamService.UserExistsAsync(command.UserId.Value))
            throw new ArgumentException($"User {command.UserId} not found");

        // 2. Fetch profile info
        var email = await externalProfileService.FetchProfileEmail(command.UserId.Value);
        var fullName = await externalProfileService.FetchProfileFullName(command.UserId.Value);
        
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException($"Profile for User {command.UserId} requires an email to subscribe.");
        
        // 2. Validate that the plan exists
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId.Value));
        if (plan == null)
            throw new ArgumentException($"Plan {command.PlanId} not found");

        if (plan.GatewayPriceId == null || string.IsNullOrEmpty(plan.GatewayPriceId.Value))
            throw new InvalidOperationException($"Plan {plan.Name} does not have a Stripe Price ID configured");

        // 3. Verifies if the user already has an active subscription
        var existingSubscription = await subscriptionRepository.FindActiveByUserIdAsync(new UserId(command.UserId.Value));

        if (existingSubscription != null)
            throw new InvalidOperationException(
                $"User {command.UserId} already has an active subscription");

        // 4. Creates a Customer in Stripe
        var stripeCustomerId = await paymentGateway.CreateOrGetCustomerAsync(
            command.UserId.Value,
            email,
            fullName);

        // 5. Creates a Checkout session
        var checkoutUrl = await paymentGateway.CreateCheckoutSessionAsync(
            stripeCustomerId,
            new PaymentGatewayPriceId(plan.GatewayPriceId.Value),
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
    public async Task Handle(CancelSubscriptionInGatewayCommand command)
    {
        logger.LogInformation(
            "Cancelling subscription {SubscriptionId} in Stripe (Immediately: {Immediately})",
            command.SubscriptionId,
            command.Immediately);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        var stripeSubscriptionId = new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value);

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
    public async Task Handle(ChangeSubscriptionPlanInGatewayCommand command)
    {
        logger.LogInformation(
            "Changing subscription plan {SubscriptionId} to Plan {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        // 2. Find the new plan
        var newPlanId = new PlanId(command.NewPlanId.Value);
        var newPlan = await planRepository.FindByIdAsync(newPlanId);
        if (newPlan == null)
            throw new ArgumentException($"Plan {command.NewPlanId} not found");

        if (newPlan.GatewayPriceId == null || string.IsNullOrEmpty(newPlan.GatewayPriceId.Value))
            throw new InvalidOperationException($"Plan {newPlan.Name} does not have a Stripe Price ID");

        // 3. Updates in Stripe
        var stripeSubscriptionId = new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value);
        var updatedStripeSubscription = await paymentGateway.UpdateSubscriptionPlanAsync(
            stripeSubscriptionId,
            new PaymentGatewayPriceId(newPlan.GatewayPriceId.Value),
            command.ProrationBehavior);

        // 4. Update in our DB
        subscription.ChangePlan(
            newPlanId,
            updatedStripeSubscription.currentPeriodEnd);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription plan {SubscriptionId} updated to {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);
    }
}