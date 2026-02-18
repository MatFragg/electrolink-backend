using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

public class ServiceRequestQueryService(
    IServiceRequestRepository requestRepository,
    ILogger<ServiceRequestQueryService> logger)
    : IServiceRequestQueryService
{
    public async Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting request {query.RequestId} for homeowner {query.HomeownerId}");
        
        var request = await requestRepository.FindByIdAsync(new RequestId(query.RequestId));
        
        // Validar ownership
        if (request != null && request.HomeownerId.Id != query.HomeownerId)
        {
            logger.LogWarning($"[Planning BC] Homeowner {query.HomeownerId} attempted to access request {query.RequestId} owned by {request.HomeownerId.Id}");
            return null;
        }
        
        return request;
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetAllRequestsByHomeownerQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting all requests for homeowner {query.HomeownerId}");
        return await requestRepository.FindByHomeownerIdAsync(new HomeownerId(query.HomeownerId));
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetRequestsByStatusQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting requests by status {query.Status} for homeowner {query.HomeownerId}");
        
        var allRequests = await requestRepository.FindByHomeownerIdAsync(new HomeownerId(query.HomeownerId));
        
        // Filtrar por status
        if (Enum.TryParse<RequestStatus>(query.Status, true, out var status))
        {
            return allRequests.Where(r => r.Status == status).ToList();
        }
        
        return Enumerable.Empty<ServiceRequest>();
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetPendingAssignmentRequestsQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting all pending assignment requests");
        return await requestRepository.FindPendingAssignmentsAsync();
    }
}

