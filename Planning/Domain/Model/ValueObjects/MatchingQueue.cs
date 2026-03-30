namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record MatchingQueue(
    IReadOnlyList<QueuedRequest> PriorityQueue,
    IReadOnlyList<QueuedRequest> NormalQueue,
    int TotalPending);