using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record RecordCircuitToggleCommand(
    ServiceExecutionId ExecutionId,
    DeviceId DeviceId,
    ERelayState TargetState,
    ERelayActionStatus ActionStatus,
    string? FailureReason
);
