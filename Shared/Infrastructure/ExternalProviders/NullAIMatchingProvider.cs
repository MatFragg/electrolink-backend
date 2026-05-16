using Hampcoders.Electrolink.API.Shared.Infrastructure;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.ExternalProviders;

public class NullAIMatchingProvider : IAIMatchingProvider
{
    public Task<IReadOnlyList<ScoredCandidate>> ScoreTechnicianCandidatesAsync(
        MatchingContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<ScoredCandidate>>([]);
    }
}
