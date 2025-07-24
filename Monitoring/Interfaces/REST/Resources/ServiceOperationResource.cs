namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record ServiceOperationResource(
    Guid RequestId,
    string CurrentStatus,
    DateTime StartedAt,
    DateTime? CompletedAt);