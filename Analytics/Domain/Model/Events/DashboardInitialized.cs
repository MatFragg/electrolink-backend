using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record DashboardInitialized(
    string DashboardId,
    string HomeownerId,
    string PropertyId,
    string PlanTier,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
