using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

public record CircuitToggleRequestedEvent(
    ServiceExecutionId ExecutionId,
    DeviceId DeviceId,
    ERelayState TargetState,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
