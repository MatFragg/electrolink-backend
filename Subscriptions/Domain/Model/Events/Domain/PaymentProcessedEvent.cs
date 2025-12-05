using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

/// <summary>
/// Represents the event when a payment transaction is processed (successful, failed, or pending).
/// </summary>
public record PaymentProcessedEvent(
    Guid TransactionId,
    Guid SubscriptionId,
    decimal Amount,
    EPaymentStatus Status,
    string GatewayTransactionId,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}