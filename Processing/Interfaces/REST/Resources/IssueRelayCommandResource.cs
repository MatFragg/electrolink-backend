namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record IssueRelayCommandResource(
    string TargetRelayState,
    string PropertyId,
    string ServiceRequestId
);
