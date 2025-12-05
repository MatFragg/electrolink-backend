using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

/// <summary>
/// Represents the event when a subscription's status changes (e.g., Active to Cancelled, Trial to Active).
/// </summary>
public record SubscriptionStatusChangedEvent(
    Guid SubscriptionId,
    UserId UserId,
    ESubscriptionStatus OldStatus,
    ESubscriptionStatus NewStatus,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}