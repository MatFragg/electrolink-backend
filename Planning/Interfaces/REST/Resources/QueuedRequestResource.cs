namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record QueuedRequestResource(
    string RequestId,
    string HomeownerId,
    bool IsPriority,
    string CreatedAt);