using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: User changed billing cycle (Monthly ↔ Annual).
/// Webhook: customer.subscription.updated
/// 
/// Consumed by: Notifications BC (send confirmation), Monitoring BC (audit trail)
/// </summary>
public record SubscriptionBillingCycleChangedEvent(
    string SubscriptionId,
    string UserId,
    string PreviousCycle,
    string NewCycle,
    DateTime NewPeriodEnd,
    DateTime ChangedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ChangedAt;
}
