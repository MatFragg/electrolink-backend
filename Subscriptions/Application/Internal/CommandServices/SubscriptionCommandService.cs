using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
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
    IStripeService stripeService,
    IUnitOfWork unitOfWork,
    IMediator mediator) : ISubscriptionCommandService
{
    public async Task<Subscription> Handle(CreateSubscriptionCommand command)
    {
        if (await subscriptionRepository.ExistsByUserIdAsync(command.UserId))
            throw new InvalidOperationException($"Subscription already exists for user {command.UserId}.");

        var businessRole = BusinessRole.From(command.BusinessRole);
        var stripeCustomerId = await stripeService.CreateCustomerAsync(command.UserId.Value);

        var subscription = Subscription.Initialize(
            command.UserId,
            businessRole,
            StripeCustomerId.From(stripeCustomerId));

        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<string> Handle(InitiateCheckoutCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);
        var billingCycle = BillingCycle.From(command.BillingCycle);

        var (sessionId, checkoutUrl) = await stripeService.CreateCheckoutSessionAsync(
            subscription.StripeCustomerId.Value,
            subscription.BusinessRole.Value,
            billingCycle.Value,
            command.SuccessUrl,
            command.CancelUrl);

        subscription.InitiateCheckout(billingCycle, StripeCheckoutSessionId.From(sessionId));

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return checkoutUrl;
    }

    public async Task<Subscription> Handle(ActivateSubscriptionCommand command)
    {
        if (await paymentRecordRepository.ExistsByStripeInvoiceIdAsync(command.StripeInvoiceId))
            return await subscriptionRepository.FindByStripeCustomerIdOrFailAsync(command.StripeCustomerId);

        var subscription = await subscriptionRepository.FindByStripeCustomerIdOrFailAsync(command.StripeCustomerId);

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

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(RecordSuccessfulRenewalCommand command)
    {
        if (await paymentRecordRepository.ExistsByStripeInvoiceIdAsync(command.StripeInvoiceId))
            return await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdOrFailAsync(command.StripeSubscriptionId);

        subscription.RecordRenewal(
            BillingPeriod.Of(command.NewPeriodStart, command.NewPeriodEnd),
            StripeInvoiceId.From(command.StripeInvoiceId));

        var paymentRecord = PaymentRecord.Create(
            subscription.SubscriptionId,
            StripeInvoiceId.From(command.StripeInvoiceId),
            Money.Of(command.AmountPaid, command.Currency),
            PaymentStatus.Succeeded,
            DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
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

        subscriptionRepository.Update(subscription);
        await paymentRecordRepository.AddAsync(paymentRecord);
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

        await stripeService.CancelAtPeriodEndAsync(subscription.StripeSubscriptionId!.Value);

        subscription.ScheduleCancellation(command.Reason, DateTime.UtcNow);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        await PublishAndClearAsync(subscription);

        return subscription;
    }

    public async Task<Subscription> Handle(IncrementMonthlyRequestCounterCommand command)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(command.UserId);

        subscription.IncrementRequestCounter();

        subscriptionRepository.Update(subscription);
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

    private async Task PublishAndClearAsync(Subscription subscription)
    {
        foreach (var domainEvent in subscription.DomainEvents)
            await mediator.Publish(domainEvent);

        subscription.ClearDomainEvents();
    }
}