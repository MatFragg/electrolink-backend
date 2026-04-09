using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when a new service execution is created.
/// </summary>
public record ServiceExecutionCreatedEvent(
    ServiceExecutionId ExecutionId,
    AssignmentId AssignmentId,
    TechnicianId TechnicianId,
    HomeownerId HomeownerId,
    DateTime OccurredOn
);

