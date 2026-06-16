using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

public record CircuitToggleRecordedEvent(
    ServiceExecutionId ExecutionId,
    DeviceId DeviceId,
    ERelayState TargetState,
    ERelayActionStatus ActionStatus,
    string? FailureReason,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
