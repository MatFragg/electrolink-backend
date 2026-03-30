namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record MatchingResultResource(
    string RequestId,
    bool Assigned,
    string? FailureReason);