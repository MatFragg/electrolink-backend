namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

// ServiceAssignment Queries
public record GetServiceAssignmentByIdQuery(Guid ServiceId);

public record GetAssignmentsByTechnicianQuery(Guid TechnicianId);

public record GetAssignmentsByHomeownerQuery(Guid HomeownerId);

public record GetAssignmentByRequestIdQuery(Guid RequestId);

