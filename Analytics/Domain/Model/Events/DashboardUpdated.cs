using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record DashboardUpdated(
    string DashboardId,
    string HomeownerId,
    decimal LatestKilowattHours,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
