namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record MatchingScore
{
    public double Score { get; }
    public MatchingMethod Method { get; }
    public string? AIReasoning { get; }

    private MatchingScore(double score, MatchingMethod method, string? reasoning)
    {
        if (score < 0 || score > 1)
            throw new ArgumentException("Score must be between 0 and 1", nameof(score));

        Score = score;
        Method = method;
        AIReasoning = reasoning;
    }

    public static MatchingScore Create(double score, MatchingMethod method, string? reasoning = null)
        => new(score, method, reasoning);

    public static MatchingScore Deterministic(double score)
        => new(score, MatchingMethod.Deterministic, null);

    public static MatchingScore AIEnhanced(double score, string reasoning)
        => new(score, MatchingMethod.AIEnhanced, reasoning);

    public static MatchingScore AIFallback(double score)
        => new(score, MatchingMethod.AIFallback, null);
}
