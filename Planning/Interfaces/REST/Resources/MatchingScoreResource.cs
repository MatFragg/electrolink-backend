namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record MatchingScoreResource(
    double Score,
    string Method,
    string? Reasoning
);
