using System.Diagnostics;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure;
using Hampcoders.Electrolink.API.Shared.Infrastructure.ExternalProviders;
using Microsoft.Extensions.Options;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class HybridMatchingService : IMatchingService
{
    private readonly IAIMatchingProvider _aiProvider;
    private readonly OpenAISettings _settings;
    private readonly ExternalProfilesService _profilesService;
    private readonly ILogger<HybridMatchingService> _logger;

    private const double DistanceWeight = 0.30;
    private const double RatingWeight = 0.25;
    private const double ExperienceWeight = 0.20;
    private const double ResponseTimeWeight = 0.15;
    private const double IoTBonusWeight = 0.10;
    private const double MaxDistanceKm = 100.0;
    private const int MaxCompletedServices = 500;
    private const int MaxResponseTimeMinutes = 120;

    public HybridMatchingService(
        IAIMatchingProvider aiProvider,
        IOptions<OpenAISettings> settings,
        ExternalProfilesService profilesService,
        ILogger<HybridMatchingService> logger)
    {
        _aiProvider = aiProvider;
        _settings = settings.Value;
        _profilesService = profilesService;
        _logger = logger;
    }

    public async Task<MatchingResult> FindBestCandidateAsync(
        RequestId requestId,
        IReadOnlyList<EnrichedCandidate> candidates,
        ServiceRequest request,
        CancellationToken ct = default)
    {
        var stopwatch = Stopwatch.StartNew();

        var filtered = HardFilterCandidates(candidates.ToList(), request);

        if (!filtered.Any())
        {
            _logger.LogWarning("No candidates passed hard filtering for RequestId {RequestId}", requestId.Value);
            throw new InvalidOperationException("No candidates available after filtering.");
        }

        var cappedCandidates = filtered
            .OrderByDescending(c => c.Rating)
            .Take(_settings.MaxCandidatesForAI)
            .ToList();

        var context = BuildMatchingContext(cappedCandidates, request);

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_settings.MatchingTimeoutSeconds));

            var aiScored = await _aiProvider.ScoreTechnicianCandidatesAsync(context, cts.Token);

            if (aiScored.Any())
            {
                stopwatch.Stop();
                var topAi = aiScored.OrderByDescending(s => s.Score).First();
                var bestCandidate = cappedCandidates.First(c => c.TechnicianId == topAi.TechnicianId);
                var score = MatchingScore.AIEnhanced(topAi.Score, topAi.Reasoning);

                _logger.LogInformation(
                    "AI Enhanced Matching: Method={Method}, Score={Score}, Latency={Latency}ms, RequestId={RequestId}",
                    MatchingMethod.AIEnhanced, score.Score, stopwatch.ElapsedMilliseconds, requestId.Value);

                return new MatchingResult(bestCandidate, aiScored, score, stopwatch.Elapsed);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("AI matching timed out after {Timeout}s for RequestId {RequestId}, falling back to deterministic",
                _settings.MatchingTimeoutSeconds, requestId.Value);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "AI matching failed for RequestId {RequestId}, falling back to deterministic", requestId.Value);
        }

        stopwatch.Restart();
        var deterministicScored = ScoreDeterministically(cappedCandidates, request);
        stopwatch.Stop();

        var topDeterministic = deterministicScored.OrderByDescending(s => s.Score).First();
        var bestDetCandidate = cappedCandidates.First(c => c.TechnicianId == topDeterministic.TechnicianId);
        var detScore = MatchingScore.AIFallback(topDeterministic.Score);

        _logger.LogInformation(
            "Deterministic Fallback Matching: Method={Method}, Score={Score}, Latency={Latency}ms, RequestId={RequestId}",
            MatchingMethod.AIFallback, detScore.Score, stopwatch.ElapsedMilliseconds, requestId.Value);

        return new MatchingResult(bestDetCandidate, deterministicScored, detScore, stopwatch.Elapsed);
    }

    private List<EnrichedCandidate> HardFilterCandidates(
        List<EnrichedCandidate> candidates,
        ServiceRequest request)
    {
        var filtered = candidates.Where(c => c.HasRequiredComponents).ToList();

        if (request.IsPriority)
        {
            filtered = filtered.Where(c => c.ResponseTimeMinutesAvg <= 60).ToList();
        }

        return filtered;
    }

    private MatchingContext BuildMatchingContext(
        List<EnrichedCandidate> candidates,
        ServiceRequest request)
    {
        var technicianCandidates = candidates.Select(c => new TechnicianCandidate(
            c.TechnicianId,
            c.DistanceKm,
            c.Rating,
            c.CompletedServicesCount,
            c.IsIoTCertified,
            c.Specialties,
            c.HasRequiredComponents,
            c.ResponseTimeMinutesAvg
        )).ToList();

        return new MatchingContext(
            request.RequestedCategory?.ToString() ?? "General",
            request.Preferences?.ProblemDescription ?? string.Empty,
            RequiresIoTCertification: request.RequiresIoTCertifiedTechnician,
            request.Geolocation!,
            technicianCandidates,
            PriorAnomalyDescription: null
        );
    }

    private IReadOnlyList<ScoredCandidate> ScoreDeterministically(
        List<EnrichedCandidate> candidates,
        ServiceRequest request)
    {
        var scored = new List<ScoredCandidate>();

        foreach (var candidate in candidates)
        {
            var distanceScore = Math.Max(0, 1.0 - (candidate.DistanceKm / MaxDistanceKm));
            var ratingScore = candidate.Rating / 5.0;
            var experienceScore = Math.Min(1.0, candidate.ExperienceYears / 10.0);
            var responseTimeScore = Math.Max(0, 1.0 - (candidate.ResponseTimeMinutesAvg / (double)MaxResponseTimeMinutes));
            var iotScore = candidate.IsIoTCertified ? 1.0 : 0.0;

            var totalScore = (distanceScore * DistanceWeight) +
                             (ratingScore * RatingWeight) +
                             (experienceScore * ExperienceWeight) +
                             (responseTimeScore * ResponseTimeWeight) +
                             (iotScore * IoTBonusWeight);

            totalScore = Math.Clamp(totalScore, 0.0, 1.0);

            var reasoning = $"Distance: {candidate.DistanceKm:F1}km ({distanceScore:F2}), " +
                            $"Rating: {candidate.Rating:F1}/5 ({ratingScore:F2}), " +
                            $"Experience: {candidate.ExperienceYears}y ({experienceScore:F2}), " +
                            $"Response: {candidate.ResponseTimeMinutesAvg}min ({responseTimeScore:F2}), " +
                            $"IoT: {(candidate.IsIoTCertified ? "Yes" : "No")} ({iotScore:F2})";

            scored.Add(new ScoredCandidate(candidate.TechnicianId, totalScore, reasoning));
        }

        return scored;
    }
}
