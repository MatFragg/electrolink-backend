using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

public class ServiceAssignmentQueryService(
    IServiceAssignmentRepository assignmentRepository,
    IServiceRequestRepository requestRepository,
    ILogger<ServiceAssignmentQueryService> logger)
    : IServiceAssignmentQueryService
{
    public async Task<ServiceAssignment?> Handle(GetServiceAssignmentByIdQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting assignment {query.ServiceId}");
        return await assignmentRepository.FindByIdAsync(new ServiceId(query.ServiceId));
    }

    public async Task<IEnumerable<ServiceAssignment>> Handle(GetAssignmentsByTechnicianQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting assignments for technician {query.TechnicianId}");
        return await assignmentRepository.FindByTechnicianIdAsync(new TechnicianId(query.TechnicianId));
    }

    public async Task<IEnumerable<ServiceAssignment>> Handle(GetAssignmentsByHomeownerQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting assignments for homeowner {query.HomeownerId}");
        
        // Obtener todos los requests del homeowner
        var requests = await requestRepository.FindByHomeownerIdAsync(new HomeownerId(query.HomeownerId));
        var assignedRequestIds = requests
            .Where(r => r.AssignedServiceId != null)
            .Select(r => r.AssignedServiceId!.Id)
            .ToList();
        
        // Buscar assignments correspondientes
        var assignments = new List<ServiceAssignment>();
        foreach (var serviceId in assignedRequestIds)
        {
            var assignment = await assignmentRepository.FindByIdAsync(new ServiceId(serviceId));
            if (assignment != null)
                assignments.Add(assignment);
        }
        
        return assignments;
    }

    public async Task<ServiceAssignment?> Handle(GetAssignmentByRequestIdQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting assignment for request {query.RequestId}");
        return await assignmentRepository.FindByRequestIdAsync(new RequestId(query.RequestId));
    }
}

