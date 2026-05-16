using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure;

public record TechnicianCandidate(
    string TechnicianId,
    double DistanceKm,
    double AverageRating,
    int CompletedServicesCount,
    bool IsIoTCertified,
    IReadOnlyList<string> Specialties,
    bool HasRequiredComponents,
    int ResponseTimeMinutesAvg
);

public record MatchingContext(
    string ServiceCategory,
    string ServiceDescription,
    bool RequiresIoTCertification,
    Geolocation PropertyLocation,
    IReadOnlyList<TechnicianCandidate> Candidates,
    string? PriorAnomalyDescription
);

public record ScoredCandidate(string TechnicianId, double Score, string Reasoning);

public interface IAIMatchingProvider
{
    Task<IReadOnlyList<ScoredCandidate>> ScoreTechnicianCandidatesAsync(
        MatchingContext context,
        CancellationToken cancellationToken = default);
}
