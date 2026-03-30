namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record QueuedRequest(string RequestId, string HomeownerId, bool IsPriority, DateTimeOffset CreatedAt);
