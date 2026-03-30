namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record MatchingQueueResource(
    IReadOnlyList<QueuedRequestResource> PriorityQueue,
    IReadOnlyList<QueuedRequestResource> NormalQueue,
    int TotalPending);