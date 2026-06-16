using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

public record ActiveServiceAnomalyAlertedEvent(
    ServiceExecutionId ExecutionId,
    DeviceId DeviceId,
    string AnomalyType,
    string Description,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
