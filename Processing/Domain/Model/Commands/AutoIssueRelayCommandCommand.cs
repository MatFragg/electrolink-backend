namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record AutoIssueRelayCommandCommand(
    string  DeviceId,
    string  PropertyId,
    string  AnomalyId
);
