namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record ServiceAssignmentResource(
    string AssignmentId,
    string RequestId,
    string TechnicianId,
    RecipeSnapshotResource RecipeSnapshot,
    string Status,
    string? FailureReason,
    int RetryCount,
    DateTime AssignedAt
);

