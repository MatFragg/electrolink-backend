using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when a technician submits a review.
/// </summary>
public record TechnicianReviewSubmittedEvent(
    ServiceExecutionId ExecutionId,
    AssignmentId AssignmentId,
    string ReviewerId,
    string ReviewedId,
    int Rating,
    Dictionary<string, int> Categories,
    DateTime SubmittedAt,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

