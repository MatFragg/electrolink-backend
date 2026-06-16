using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record DashboardTierUpgraded(
    string DashboardId,
    string HomeownerId,
    string NewPlanTier,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
