using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Application.ACL;

/// <inheritdoc />
public sealed class MonitoringContextFacade(
    IServiceExecutionCommandService commandService,
    IProfilesContextFacade profilesContextFacade,   
    IUnitOfWork unitOfWork)
    : IMonitoringContextFacade
{
    public async Task<int> CountActiveServicesForRecipeAsync(string recipeId)
    {
        /*var executions = await executionRepository.FindByRecipeIdAsync(recipeId);
        return executions.Count(e =>
            e.Status is EExecutionStatus.Scheduled or EExecutionStatus.InProgress);*/
        return 0;
    }

    public async Task<int> CountInProgressServicesForRecipeAsync(string recipeId)
    {
        /*var executions = await executionRepository.FindByRecipeIdAsync(recipeId);
        return executions.Count(e => e.Status == EExecutionStatus.InProgress);*/
        return 0;
    }

    public async Task<bool> IsServiceActiveAsync(string serviceId)
    {
        /*var execution = await executionRepository.FindByServiceIdAsync(serviceId);
        return execution is not null &&
               execution.Status is EExecutionStatus.Scheduled or EExecutionStatus.InProgress;*/
        return false;
    }

    public async Task<string?> GetServiceExecutionStatusAsync(string serviceId)
    {
        /*var execution = await executionRepository.FindByServiceIdAsync(serviceId);
        return execution?.Status.ToString();*/ 
        return null;
    }

    public async Task<string> CreateServiceExecutionAsync(
        string assignmentId,
        string requestId,
        string technicianId,
        string homeownerId,
        string propertyId,
        RecipeSnapshot recipeSnapshot,
        DateTime scheduledAt,
        bool isPriority)
    {
        var command = new CreateServiceExecutionCommand(
            AssignmentId.From(assignmentId), RequestId.From(requestId), TechnicianId.From(technicianId), HomeownerId.From(homeownerId), PropertyId.From(propertyId),
            recipeSnapshot, scheduledAt, isPriority);

        var execution = await commandService.Handle(command);
        return execution.Id.Value;
    }
}