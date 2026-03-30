using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Application.ACL;

/// <inheritdoc />
public sealed class MonitoringContextFacade(
    IServiceOperationCommandService   operationCmdService,
    IServiceOperationRepository serviceOperationRepository,
    IProfilesContextFacade profilesContextFacade,   
    IUnitOfWork                       unitOfWork)
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
}