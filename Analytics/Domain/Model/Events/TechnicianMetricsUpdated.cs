using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record TechnicianMetricsUpdated(
    string MetricsId,
    string TechnicianId,
    int CompletedServicesCount,
    decimal TotalRevenue,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
