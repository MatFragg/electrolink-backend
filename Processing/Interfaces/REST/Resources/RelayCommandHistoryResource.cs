namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record RelayCommandHistoryResource(
    string   DeviceId,
    string   CurrentRelayState,
    DateTime? LastCommandAt,
    IReadOnlyList<RelayCommandItemResource> Commands
);

public record RelayCommandItemResource(
    string   CommandId,
    string   TargetRelayState,
    string   RequestedBy,
    string   AuthorizationSource,
    string?  ServiceRequestId,
    string   Status,
    DateTime IssuedAt,
    DateTime? ExecutedAt,
    string?  FailureReason
);
