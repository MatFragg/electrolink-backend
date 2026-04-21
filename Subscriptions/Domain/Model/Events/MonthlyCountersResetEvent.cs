using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Monthly counters reset job executed (1st day of month, 00:00 UTC).
/// Summary event for audit/monitoring.
/// 
/// Consumed by: Monitoring BC (track reset activity)
/// </summary>
public record MonthlyCountersResetEvent(
    DateTime ResetDate,
    int SubscriptionsReset,
    DateTime ResetAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ResetAt;
}
