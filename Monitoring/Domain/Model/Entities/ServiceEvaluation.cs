using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class ServiceEvaluation
{
    public EvaluationId Id { get; private set; }
    public ServiceExecutionId ExecutionId { get; private set; }
    public string ReviewerId { get; private set; }
    public string ReviewedId { get; private set; }
    public string ReviewerRole { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public string CategoriesJson { get; private set; }
    public DateTime SubmittedAt { get; private set; }

    private ServiceEvaluation() { }

    public static ServiceEvaluation Create(
        ServiceExecutionId executionId,
        string reviewerId,
        string reviewedId,
        string reviewerRole,
        int rating,
        string? comment,
        Dictionary<string, int> categories,
        DateTime submittedAt)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        return new ServiceEvaluation
        {
            Id = EvaluationId.NewId(),
            ExecutionId = executionId,
            ReviewerId = reviewerId,
            ReviewedId = reviewedId,
            ReviewerRole = reviewerRole,
            Rating = rating,
            Comment = comment,
            CategoriesJson = System.Text.Json.JsonSerializer.Serialize(categories),
            SubmittedAt = submittedAt,
        };
    }
}
