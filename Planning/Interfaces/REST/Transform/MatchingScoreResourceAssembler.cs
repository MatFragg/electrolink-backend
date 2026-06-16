using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class MatchingScoreResourceAssembler
{
    public static MatchingScoreResource ToResource(MatchingScore score)
    {
        return new MatchingScoreResource(
            score.Score,
            score.Method.ToString(),
            score.AIReasoning
        );
    }
}
