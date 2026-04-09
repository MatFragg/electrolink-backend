namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record CreateServiceExecutionResource(
    string AssignmentId,     
    string RequestId,
    string TechnicianId,
    string HomeownerId,
    string PropertyId,
    RecipeSnapshotResource RecipeSnapshot,
    DateTime ScheduledDateTime,
    bool IsPriority
);