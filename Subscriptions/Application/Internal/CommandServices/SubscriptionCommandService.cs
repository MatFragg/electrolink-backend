using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRecordRepository paymentRecordRepository,
    IPaymentGatewayService paymentGateway,
    IUnitOfWork unitOfWork,
    ExternalProfileService externalProfileService,
    ExternalIamService externalIamService,
    IMediator mediator
) : ISubscriptionCommandService
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command)
    {
        if (!int.TryParse(command.UserId.Value, out var localUserId))
            throw new ArgumentException($"Unsupported UserId format for Subscriptions BC: {command.UserId.Value}");

        var subscription = new Subscription(
            new UserId(localUserId),
            new PlanId(command.PlanId),
            command.StartDate,
            command.EndDate,
            command.GatewayCustomerId,
            command.GatewaySubscriptionId,
            command.InitialStatus,
            command.TrialEndsAt);

        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription.Id.Value;
    }

    public async Task<Guid?> Handle(UpdateSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.UpdateStatus(command.NewStatus);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription.Id.Value;
    }

    public async Task<Guid?> Handle(IncrementSubscriptionUsageCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.IncrementUsage();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription.Id.Value;
    }

    public async Task<Guid?> Handle(ChangeSubscriptionPlanCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null) return null;

        var newPlanId = new PlanId(command.NewPlanId);
        subscription.ChangePlan(newPlanId, command.NewEndDate);
        subscription.UpdateStripeSubscriptionId(command.GatewaySubscriptionId);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription.Id.Value;
    }

    public async Task<string> Handle(CreateCheckoutSessionCommand command)
    {
        if (!int.TryParse(command.UserId, out var localUserId))
            throw new ArgumentException($"Unsupported UserId format for checkout: {command.UserId}");

        if (!await externalIamService.UserExistsAsync(command.UserId))
            throw new ArgumentException($"User {command.UserId} not found.");

        if (!await externalProfileService.ProfileExistsAsync(command.UserId))
            throw new ArgumentException($"Profile for user {command.UserId} not found.");

        var fullName = await externalProfileService.FetchProfileFullName(command.UserId);
        var syntheticEmail = $"{command.UserId}@electrolink.local";

        var existing = await subscriptionRepository.FindActiveByUserIdAsync(new UserId(localUserId));
        if (existing != null)
            throw new InvalidOperationException($"User {command.UserId} already has an active subscription.");

        var stripeCustomerId = await paymentGateway.CreateOrGetCustomerAsync(localUserId, syntheticEmail, fullName);

        return await paymentGateway.CreateCheckoutSessionAsync(
            stripeCustomerId,
            command.PriceId,
            command.SuccessUrl,
            command.CancelUrl,
            command.TrialPeriodDays);
    }

    public async Task Handle(CancelSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId.Value} not found.");

        if (subscription.GatewaySubscriptionId == null)
            throw new InvalidOperationException("Cannot cancel a BASIC subscription without Stripe subscription id.");

        var stripeSubscriptionId = new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value);

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

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(ChangeSubscriptionPlanInGatewayCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId.Value} not found.");

        if (subscription.GatewaySubscriptionId == null)
            throw new InvalidOperationException("Cannot change plan in gateway without Stripe subscription id.");

        var updatedStripeSubscription = await paymentGateway.UpdateSubscriptionPlanAsync(
            new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value),
            command.NewPriceId,
            command.ProrationBehavior);

        subscription.ChangePlan(subscription.PlanId, updatedStripeSubscription.currentPeriodEnd);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(SyncSubscriptionFromGatewayCommand command)
    {
        var subscription = await subscriptionRepository
            .FindByPaymentGatewaySubscriptionIdAsync(command.GatewaySubscriptionId);

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for gateway ID {command.GatewaySubscriptionId.Value}.");

        var mappedStatus = MapGatewayStatusToSubscriptionStatus(command.Status);

        subscription.UpdateStatus(mappedStatus);
        subscription.UpdateEndDate(command.CurrentPeriodEnd);
        subscription.UpdateTrialEndsAt(command.TrialEnd);

        if (command.CancelAtPeriodEnd && command.CancelAt.HasValue)
            subscription.ScheduleCancellation(command.CancelAt.Value);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task<string> Handle(CreateBillingPortalSessionCommand command)
    {
        var subscription = await subscriptionRepository.FindActiveByUserIdAsync(command.UserId);
        if (subscription == null)
            throw new ArgumentException($"No active subscription found for user {command.UserId.Value}.");

        return await paymentGateway.CreateBillingPortalSessionAsync(subscription.GatewayCustomerId, command.ReturnUrl);
    }

    public async Task Handle(ActivateSubscriptionCommand command)
    {
        if (await paymentRecordRepository.ExistsByStripeInvoiceIdAsync(StripeInvoiceId.From(command.StripeInvoiceId)))
            return;

        var subscription = await subscriptionRepository.FindByPaymentGatewayCustomerIdAsync(
            new PaymentGatewayCustomerId(command.StripeCustomerId));

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for customer {command.StripeCustomerId}.");

        var billingCycle = BillingCycle.From(command.BillingCycle);
        var period = BuildPeriod(command.PeriodStart, command.PeriodEnd, billingCycle);

        subscription.Activate(new PaymentGatewaySubscriptionId(command.StripeSubscriptionId), billingCycle, period);

        var paymentRecord = PaymentRecord.Create(
            subscription.Id,
            StripeInvoiceId.From(command.StripeInvoiceId),
            command.AmountPaid,
            command.Currency,
            EPaymentStatus.Success,
            DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(RecordSuccessfulRenewalCommand command)
    {
        if (await paymentRecordRepository.ExistsByStripeInvoiceIdAsync(StripeInvoiceId.From(command.StripeInvoiceId)))
            return;

        var subscription = await subscriptionRepository.FindByPaymentGatewaySubscriptionIdAsync(
            new PaymentGatewaySubscriptionId(command.StripeSubscriptionId));

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for Stripe subscription {command.StripeSubscriptionId}.");

        var period = BuildPeriod(command.NewPeriodStart, command.NewPeriodEnd, subscription.BillingCycle ?? BillingCycle.Monthly());

        subscription.RecordRenewal(period);
        subscription.UpdateEndDate(period.PeriodEnd);

        var paymentRecord = PaymentRecord.Create(
            subscription.Id,
            StripeInvoiceId.From(command.StripeInvoiceId),
            command.AmountPaid,
            command.Currency,
            EPaymentStatus.Success,
            DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(StartGracePeriodCommand command)
    {
        if (await paymentRecordRepository.ExistsByStripeInvoiceIdAsync(StripeInvoiceId.From(command.StripeInvoiceId)))
            return;

        var subscription = await subscriptionRepository.FindByPaymentGatewaySubscriptionIdAsync(
            new PaymentGatewaySubscriptionId(command.StripeSubscriptionId));

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for Stripe subscription {command.StripeSubscriptionId}.");

        subscription.StartGracePeriod(command.FailedAt ?? DateTime.UtcNow);

        var paymentRecord = PaymentRecord.Create(
            subscription.Id,
            StripeInvoiceId.From(command.StripeInvoiceId),
            command.AmountDue,
            command.Currency,
            EPaymentStatus.Failed,
            DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(DegradeSubscriptionCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.StripeSubscriptionId))
            return;

        var subscription = await subscriptionRepository.FindByPaymentGatewaySubscriptionIdAsync(
            new PaymentGatewaySubscriptionId(command.StripeSubscriptionId));

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for Stripe subscription {command.StripeSubscriptionId}.");

        subscription.Degrade(command.DegradedAt ?? DateTime.UtcNow);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(UpdateBillingCycleCommand command)
    {
        var subscription = await subscriptionRepository.FindByPaymentGatewaySubscriptionIdAsync(
            new PaymentGatewaySubscriptionId(command.StripeSubscriptionId));

        if (subscription == null)
            throw new ArgumentException($"Subscription not found for Stripe subscription {command.StripeSubscriptionId}.");

        var newCycle = BillingCycle.From(command.NewBillingCycle);
        var newPeriod = BuildPeriod(command.NewPeriodStart, command.NewPeriodEnd, newCycle);

        subscription.UpdateBillingCycle(newCycle, newPeriod);
        subscription.UpdateEndDate(newPeriod.PeriodEnd);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);
    }

    public async Task Handle(ResetMonthlyRequestCountersCommand command)
    {
        var basicHomeowners = await subscriptionRepository.FindAllBasicHomeownersAsync();
        foreach (var subscription in basicHomeowners)
        {
            subscription.ResetMonthlyCounters();
            subscriptionRepository.Update(subscription);
        }

        await unitOfWork.CompleteAsync();
    }

    private async Task PublishAndClearAsync(Subscription subscription)
    {
        foreach (var domainEvent in subscription.DomainEvents)
            await mediator.Publish(domainEvent);

        subscription.ClearDomainEvents();
    }

    private static ESubscriptionStatus MapGatewayStatusToSubscriptionStatus(string gatewayStatus)
    {
        return gatewayStatus.ToLowerInvariant() switch
        {
            "active" => ESubscriptionStatus.Active,
            "trialing" => ESubscriptionStatus.Trial,
            "past_due" => ESubscriptionStatus.GracePeriod,
            "canceled" => ESubscriptionStatus.Cancelled,
            "unpaid" => ESubscriptionStatus.Expired,
            _ => ESubscriptionStatus.Pending
        };
    }

    private static BillingPeriod BuildPeriod(DateTime? start, DateTime? end, BillingCycle cycle)
    {
        var safeStart = (start ?? DateTime.UtcNow).ToUniversalTime();
        var safeEnd = (end ?? safeStart.AddMonths(cycle.Value == EBillingCycle.Annual ? 12 : 1)).ToUniversalTime();

        if (safeEnd <= safeStart)
            safeEnd = safeStart.AddMonths(cycle.Value == EBillingCycle.Annual ? 12 : 1);

        return BillingPeriod.Of(safeStart, safeEnd);
    }
}