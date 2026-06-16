using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IMatchingService
{
    Task<MatchingResult> FindBestCandidateAsync(
        RequestId requestId,
        IReadOnlyList<EnrichedCandidate> candidates,
        ServiceRequest request,
        CancellationToken ct = default);
}
