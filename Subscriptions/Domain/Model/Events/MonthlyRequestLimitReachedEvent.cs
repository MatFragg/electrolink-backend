using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: BASIC HOMEOWNER user reached monthly request limit (2/2).
/// Triggers upgrade prompt in UI.
/// 
/// Consumed by: Notifications BC (send upgrade incentive email)
/// </summary>
public record MonthlyRequestLimitReachedEvent(
    string SubscriptionId,
    string UserId,
    int MonthlyRequestsUsed,
    int MonthlyRequestsLimit,
    DateTime ReachedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ReachedAt;
}
