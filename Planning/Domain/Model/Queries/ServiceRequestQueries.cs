using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

// ServiceRequest Queries
public record GetServiceRequestByIdQuery(RequestId RequestId);
public record GetServiceRequestByHomeownerIdAndIdQuery(HomeownerId HomeownerId, RequestId RequestId);

public record GetAllRequestsByHomeownerQuery(HomeownerId HomeownerId);

public record GetRequestsByStatusQuery(HomeownerId HomeownerId, string Status);

public record GetPendingAssignmentRequestsQuery(); // Para admin/sistema

