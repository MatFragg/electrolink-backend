using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

/// <summary>
/// Represents the event when a new subscription is successfully created.
/// </summary>
public record SubscriptionCreatedEvent(
    Guid SubscriptionId,
    UserId UserId,
    PlanId PlanId,
    DateTime StartDate,
    ESubscriptionStatus InitialStatus,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}