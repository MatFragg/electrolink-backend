using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record ConsumptionThresholdExceeded(
    string DashboardId,
    string HomeownerId,
    string CircuitId,
    decimal ConsumedKilowattHours,
    decimal ThresholdKilowattHours,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
