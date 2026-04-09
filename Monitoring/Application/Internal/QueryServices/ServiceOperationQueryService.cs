using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

/// <summary>
/// Query service for retrieving service execution data.
/// Implements the IServiceExecutionQueryService interface.
/// </summary>
public class ServiceOperationQueryService(
    IServiceExecutionRepository executionRepository
) : IServiceExecutionQueryService
{
    /// <summary>
    /// Handles GetServiceExecutionByIdQuery to retrieve a service execution by its ID.
    /// </summary>
    public async Task<ServiceExecution?> Handle(GetServiceExecutionByIdQuery query)
    {
        return await executionRepository.FindByIdAsync(query.ExecutionId);
    }

    /// <summary>
    /// Handles GetAssignedServicesByTechnicianQuery to retrieve all services for a technician.
    /// </summary>
    public async Task<IEnumerable<ServiceExecution>> Handle(GetAssignedServicesByTechnicianQuery query)
    {
        return await executionRepository.FindByTechnicianIdAsync(query.TechnicianId);
    }

    /// <summary>
    /// Handles GetActiveServiceByHomeownerQuery to retrieve the current active service for a homeowner.
    /// </summary>
    public async Task<ServiceExecution?> Handle(GetActiveServiceByHomeownerQuery query)
    {
        var executions = await executionRepository.FindByHomeownerIdAsync(query.HomeownerId);
        return executions.FirstOrDefault(e => 
            e.Status == EExecutionStatus.Scheduled || e.Status == EExecutionStatus.InProgress);
    }

    /// <summary>
    /// Handles GetServiceHistoryByHomeownerQuery to retrieve all services for a homeowner.
    /// </summary>
    public async Task<IEnumerable<ServiceExecution>> Handle(GetServiceHistoryByHomeownerQuery query)
    {
        return await executionRepository.FindByHomeownerIdAsync(query.HomeownerId);
    }

    /// <summary>
    /// Handles GetServiceHistoryByTechnicianQuery to retrieve all services for a technician.
    /// </summary>
    public async Task<IEnumerable<ServiceExecution>> Handle(GetServiceHistoryByTechnicianQuery query)
    {
        return await executionRepository.FindByTechnicianIdAsync(query.TechnicianId);
    }

    /// <summary>
    /// Handles GetWorkLogQuery to retrieve the work log (photos, components, reports) for a service.
    /// </summary>
    public async Task<ServiceExecution?> Handle(GetWorkLogQuery query)
    {
        return await executionRepository.FindByIdAsync(query.ExecutionId);
    }

    /// <summary>
    /// Handles GetNoShowAlertQuery to retrieve services with potential no-show incidents.
    /// </summary>
    public async Task<IEnumerable<ServiceExecution>> Handle(GetNoShowAlertQuery query)
    {
        var active = await executionRepository.FindActiveServiceExecutionAsync();
        var noShowThreshold = DateTime.UtcNow.AddMinutes(-30);

        return active.Where(e =>
            e.Status == EExecutionStatus.Scheduled &&
            e.ScheduledDateTime < noShowThreshold &&
            e.StartedAt == null);
    }
}

