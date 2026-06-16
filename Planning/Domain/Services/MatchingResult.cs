using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public record MatchingResult(
    EnrichedCandidate BestCandidate,
    IReadOnlyList<ScoredCandidate> AllScored,
    MatchingScore Score,
    TimeSpan Latency
);
