using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

/// <summary>
/// Represents the event when a subscription's plan is successfully changed.
/// </summary>
public record SubscriptionPlanChangedEvent(
    Guid SubscriptionId,
    UserId UserId,
    PlanId OldPlanId,
    PlanId NewPlanId,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}