namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

// ServiceRequest Queries
public record GetServiceRequestByIdQuery(Guid RequestId, Guid HomeownerId);

public record GetAllRequestsByHomeownerQuery(Guid HomeownerId);

public record GetRequestsByStatusQuery(Guid HomeownerId, string Status);

public record GetPendingAssignmentRequestsQuery(); // Para admin/sistema

