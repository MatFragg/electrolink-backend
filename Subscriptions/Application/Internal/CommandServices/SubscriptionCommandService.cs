using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentProvider paymentProvider,
    ExternalIamService externalIamService,
    ExternalProfileService externalProfileService,
    SubscriptionPlanPriceResolver priceResolver,
    IOptions<SubscriptionSettings> settings,
    IUnitOfWork unitOfWork,
    IMediator mediator) : ISubscriptionCommandService
{
    public async Task<Subscription> Handle(CreateSubscriptionCommand command)
    {
        if (await subscriptionRepository.ExistsByUserIdAsync(command.UserId))
            throw new InvalidOperationException($"Subscription already exists for user {command.UserId}.");

        var businessRole = BusinessRole.From(command.BusinessRole);
        var email = await externalIamService.GetUserEmailAsync(command.UserId.Value);
        var name = await externalProfileService.FetchProfileFullName(command.UserId.Value)
                   ?? email.Split('@')[0];

        var externalCustomerId = await paymentProvider.CreateCustomerAsync(
            email, name, new Dictionary<string, string> { ["userId"] = command.UserId.Value }.AsReadOnly());

        var subscription = Subscription.Initialize(
            command.UserId,
            businessRole,
            StripeCustomerId.From(externalCustomerId.Value));

        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<InitiateCheckoutResult> Handle(InitiateCheckoutCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);
        var planType = PlanType.From(command.PlanType);
        var billingCycle = BillingCycle.From(command.BillingCycle);

        var priceId = planType.IsEnterprise
            ? throw new InvalidOperationException("Enterprise checkout requires special handling.")
            : priceResolver.ResolvePriceIdForRole(subscription.BusinessRole.Value, billingCycle.Value);

        var metadata = new Dictionary<string, string>
        {
            ["userId"] = command.UserId,
            ["planType"] = command.PlanType,
            ["billingCycle"] = command.BillingCycle
        }.AsReadOnly();

        var result = await paymentProvider.CreateCheckoutSessionAsync(
            new ExternalCustomerId(subscription.StripeCustomerId.Value),
            priceId,
            command.SuccessUrl,
            command.CancelUrl,
            metadata);

        subscription.InitiateCheckout(billingCycle, StripeCheckoutSessionId.From(result.SessionId));

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return new InitiateCheckoutResult(result.CheckoutUrl, result.SessionId);
    }

    public async Task<Subscription?> Handle(ActivateSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeCustomerIdOrFailAsync(command.StripeCustomerId);

        if (subscription.HasPaymentWithInvoice(command.StripeInvoiceId))
            return null;

        subscription.Activate(
            StripeSubscriptionId.From(command.StripeSubscriptionId),
            BillingCycle.From(command.BillingCycle),
            BillingPeriod.Of(command.PeriodStart, command.PeriodEnd));

        var paymentRecord = PaymentRecord.Create(
            subscription.SubscriptionId,
            StripeInvoiceId.From(command.StripeInvoiceId),
            Money.Of(command.AmountPaid, command.Currency),
            PaymentStatus.Succeeded,
            DateTime.UtcNow);

        subscription.AddPaymentRecord(paymentRecord);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription?> Handle(ActivateEnterpriseSubscriptionPendingInstallationCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeCustomerIdOrFailAsync(command.StripeCustomerId);

        if (subscription.HasPaymentWithInvoice(command.StripeInvoiceId))
            return null;

        subscription.ActivateEnterprisePendingInstallation(
            StripeSubscriptionId.From(command.StripeSubscriptionId),
            BillingPeriod.Of(command.PeriodStart, command.PeriodEnd));

        var paymentRecord = PaymentRecord.Create(
            subscription.SubscriptionId,
            StripeInvoiceId.From(command.StripeInvoiceId),
            Money.Of(command.AmountPaid, command.Currency),
            PaymentStatus.Succeeded,
            DateTime.UtcNow);

        subscription.AddPaymentRecord(paymentRecord);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription?> Handle(RecordSuccessfulRenewalCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        if (subscription.HasPaymentWithInvoice(command.StripeInvoiceId))
            return null;

        subscription.RecordRenewal(
            BillingPeriod.Of(command.NewPeriodStart, command.NewPeriodEnd),
            StripeInvoiceId.From(command.StripeInvoiceId));

        var paymentRecord = PaymentRecord.Create(
            subscription.SubscriptionId,
            StripeInvoiceId.From(command.StripeInvoiceId),
            Money.Of(command.AmountPaid, command.Currency),
            PaymentStatus.Succeeded,
            DateTime.UtcNow);

        subscription.AddPaymentRecord(paymentRecord);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(StartGracePeriodCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        subscription.StartGracePeriod(
            StripeInvoiceId.From(command.StripeInvoiceId),
            command.FailedAt);

        var paymentRecord = PaymentRecord.Create(
            subscription.SubscriptionId,
            StripeInvoiceId.From(command.StripeInvoiceId),
            Money.Of(command.AmountDue, command.Currency),
            PaymentStatus.Failed,
            command.FailedAt);

        subscription.AddPaymentRecord(paymentRecord);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(DegradeSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        subscription.Degrade(
            DegradationReason.From(command.Reason),
            command.DegradedAt);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(CancelSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);

        if (subscription.StripeSubscriptionId is not null)
        {
            await paymentProvider.CancelSubscriptionAsync(subscription.StripeSubscriptionId.Value, true);
        }

        subscription.ScheduleCancellation(command.Reason, DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(IncrementMonthlyRequestCounterCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);

        if (subscription.PlanType.IsBasic && subscription.BusinessRole.Value == EBusinessRole.Homeowner)
        {
            subscription.IncrementRequestCounter();
            subscriptionRepository.Update(subscription);
        }

        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task Handle(ResetMonthlyRequestCountersCommand command)
    {
        var subscriptions = await subscriptionRepository.FindAllBasicHomeownersAsync();
        foreach (var subscription in subscriptions)
        {
            subscription.ResetMonthlyCounters();
            subscriptionRepository.Update(subscription);
        }

        await unitOfWork.CompleteAsync();
    }

    public async Task<Subscription> Handle(UpdateBillingCycleCommand command)
    {
        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        subscription.UpdateBillingCycle(
            BillingCycle.From(command.NewBillingCycle),
            BillingPeriod.Of(command.NewPeriodStart, command.NewPeriodEnd));

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<CustomerPortalResult> Handle(OpenCustomerPortalCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);

        if (subscription.StripeCustomerId is null)
            throw new InvalidOperationException("No Stripe customer associated with this subscription.");

        var result = await paymentProvider.OpenCustomerPortalAsync(
            new ExternalCustomerId(subscription.StripeCustomerId.Value),
            command.ReturnUrl);

        return new CustomerPortalResult(result.Url);
    }

    public async Task<Subscription> Handle(CancelEnterpriseSubscriptionWithRefundCommand command)
    {
        var subscriptionId = SubscriptionId.From(command.SubscriptionId);
        var subscription = await subscriptionRepository.FindByIdAsync(subscriptionId)
            ?? throw new KeyNotFoundException($"Subscription {command.SubscriptionId} not found.");

        var lastPayment = subscription.FindLastSuccessfulPayment();
        if (lastPayment is null || lastPayment.StripePaymentIntentId is null)
            throw new InvalidOperationException("No successful payment record found for this subscription.");

        var refundResult = await paymentProvider.CreateRefundAsync(
            lastPayment.StripePaymentIntentId.Value,
            command.RefundAmount,
            "requested_by_customer");

        subscription.CancelWithRefund(refundResult.RefundId, command.Reason);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    private async Task PublishAndClearAsync(Subscription subscription)
    {
        foreach (var domainEvent in subscription.DomainEvents)
            await mediator.Publish(domainEvent);

        subscription.ClearDomainEvents();
    }
}
