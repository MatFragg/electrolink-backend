using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to submit a technician review/evaluation.
/// </summary>
public record SubmitTechnicianReviewCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId ReviewerId,
    int Rating,
    string? Comment,
    Dictionary<ETechnicianEvaluationCategory, int> Categories,
    DateTime SubmittedAt
);
