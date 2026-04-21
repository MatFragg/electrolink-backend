using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: User scheduled voluntary cancellation (cancel_at_period_end = true).
/// Will be processed into degradation at period end via batch job.
/// 
/// Consumed by: Notifications BC (send confirmation, retention offer)
/// </summary>
public record SubscriptionCancellationScheduledEvent(
    string SubscriptionId,
    string UserId,
    string BusinessRole,
    string PlanType,
    DateTime ActiveUntil,
    string Reason,
    DateTime ScheduledAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ScheduledAt;
}
