using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to submit a homeowner review/evaluation.
/// </summary>
public record SubmitHomeownerReviewCommand(
    ServiceExecutionId ExecutionId,
    HomeownerId ReviewerId,
    int Rating,
    string? Comment,
    Dictionary<EEvaluationCategory, int> Categories,
    DateTime SubmittedAt
);
