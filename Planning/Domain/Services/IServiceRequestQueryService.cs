using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceRequestQueryService
{
    Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query);
    Task<IEnumerable<ServiceRequest>> Handle(GetAllRequestsByHomeownerQuery query);
    Task<IEnumerable<ServiceRequest>> Handle(GetRequestsByStatusQuery query);
    Task<IEnumerable<ServiceRequest>> Handle(GetPendingAssignmentRequestsQuery query);
}

