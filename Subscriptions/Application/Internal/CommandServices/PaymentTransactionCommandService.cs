using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class PaymentTransactionCommandService(IPaymentTransactionRepository paymentTransactionRepository,ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IMediator mediator) : IPaymentTransactionCommandService
{
    /// <inheritdoc/>
    public async Task<Guid> Handle(ProcessPaymentCommand command)
    {
        var subscriptionId = new SubscriptionId(command.SubscriptionId.Value);
        var subscription = await subscriptionRepository.FindByIdAsync(subscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription with ID {subscriptionId} not found for payment processing.");
        
        var transaction = new PaymentTransaction(
            subscriptionId,
            command.Amount ?? 0,
            command.Currency,
            command.TransactionDate,
            command.Status,
            command.GatewayTransactionId,
            command.Message
        );  

        await paymentTransactionRepository.AddAsync(transaction);
        var oldSubscriptionStatus = subscription.Status;

        // Update subscription status based on payment result
        if (command.Status == EPaymentStatus.Success)
        {
            subscription.UpdateStatus(ESubscriptionStatus.Active);
            // Reset cancellation if a pending payment succeeded        
            subscription.UpdateEndDate(subscription.EndDate.AddMonths(1)); // Example: extend for one month
        }
        else if (command.Status == EPaymentStatus.Failed)
        {
            subscription.UpdateStatus(ESubscriptionStatus.PaymentDue); // Enter payment due state for retries
        }
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in transaction.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        transaction.ClearDomainEvents();
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        return transaction.Id; // Return the ID
    }
}