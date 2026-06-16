using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record RemotelyToggleCircuitCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    ERelayState TargetState,
    string ActorId
);
