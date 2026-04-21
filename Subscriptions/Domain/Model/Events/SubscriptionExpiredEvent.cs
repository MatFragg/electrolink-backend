using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Subscription fully expired (cancel_at_period_end processed).
/// Triggered by batch job at period end when cancel_at_period_end was true.
/// 
/// Consumed by: Monitoring BC (churn analytics), Notifications BC (winback campaign)
/// </summary>
public record SubscriptionExpiredEvent(
    string SubscriptionId,
    string UserId,
    DateTime ExpiredAt,
    string Reason) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ExpiredAt;
}
