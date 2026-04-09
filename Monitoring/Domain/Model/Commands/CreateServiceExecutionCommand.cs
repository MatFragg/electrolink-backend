using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to create a new service execution from an assignment.
/// </summary>
public record CreateServiceExecutionCommand(
    AssignmentId AssignmentId,
    RequestId RequestId,
    TechnicianId TechnicianId,
    HomeownerId HomeownerId,
    PropertyId PropertyId,
    RecipeSnapshot RecipeSnapshot,
    DateTime ScheduledDateTime,
    bool IsPriority
);


