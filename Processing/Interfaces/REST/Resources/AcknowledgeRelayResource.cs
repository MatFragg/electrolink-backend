namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record AcknowledgeRelayResource(
    string   CommandId,
    string   DeviceId,
    bool     ExecutedSuccessfully,
    DateTime ExecutedAt
);
