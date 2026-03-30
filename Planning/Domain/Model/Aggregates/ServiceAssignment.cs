using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public class ServiceAssignment : BaseAggregateRoot
{
    public AssignmentId AssignmentId { get; private set; }
    public RequestId RequestId { get; private set; }
    public TechnicianId? TechnicianId { get; private set; }
    public RecipeSnapshot? RecipeSnapshot { get; private set; }
    public MatchingCriteria? MatchingCriteria { get; private set; }
    public EAssignmentStatus Status { get; private set; }
    public string? FailureReason { get; private set; }
    public int RetryCount { get; private set; }

    private ServiceAssignment() { }

    // ── Factories ─────────────────────────────────────────

    public static ServiceAssignment Assign(
        RequestId requestId,
        TechnicianId technicianId,
        RecipeSnapshot recipeSnapshot,
        MatchingCriteria matchingCriteria)
    {
        var assignment = new ServiceAssignment
        {
            AssignmentId     = AssignmentId.NewAssignmentId(),
            RequestId        = requestId,
            TechnicianId     = technicianId,
            RecipeSnapshot   = recipeSnapshot,
            MatchingCriteria = matchingCriteria,
            Status           = EAssignmentStatus.Assigned,
            RetryCount       = 0,
        };
        assignment.RaiseDomainEvent(new ServiceAutomaticallyAssignedEvent(
            assignment.AssignmentId, requestId, technicianId, recipeSnapshot, DateTime.UtcNow));
        return assignment;
    }

    public static ServiceAssignment Fail(RequestId requestId, string reason, int retryCount)
    {
        var assignment = new ServiceAssignment
        {
            AssignmentId  = AssignmentId.NewAssignmentId(),
            RequestId     = requestId,
            Status        = EAssignmentStatus.Failed,
            FailureReason = reason,
            RetryCount    = retryCount,
        };
        assignment.RaiseDomainEvent(new ServiceAssignmentFailedEvent(requestId, reason, retryCount, DateTime.UtcNow));
        return assignment;
    }
}