namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record IssueRelayCommandCommand(
    string  DeviceId,
    string  PropertyId,
    string  TechnicianId,
    string  ServiceRequestId,
    string  TargetRelayState
);
