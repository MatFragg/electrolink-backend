using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Subscription degraded from PREMIUM to BASIC.
/// Triggered by: Grace period expiration OR voluntary cancellation at period end.
/// 
/// Consumed by:
///   - Planning BC: Deactivate ServiceCatalog for TECHNICIAN
///   - Notifications BC: Send degradation alert
///   - Monitoring BC: Audit trail, churn tracking
/// </summary>
public record SubscriptionDegradedEvent(
    string SubscriptionId,
    string UserId,
    string BusinessRole,
    string PreviousPlan,
    string NewPlan,
    string Reason,
    DateTime DegradedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DegradedAt;
}
