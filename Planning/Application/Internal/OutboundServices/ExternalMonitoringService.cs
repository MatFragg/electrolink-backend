using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

public class ExternalMonitoringService(IMonitoringContextFacade monitoringContextFacade)
{
    public async Task<int> CountActiveServicesForRecipeAsync(string recipeId)
        => await monitoringContextFacade.CountActiveServicesForRecipeAsync(recipeId);

    public async Task<int> CountInProgressServicesForRecipeAsync(string recipeId)
        => await monitoringContextFacade.CountInProgressServicesForRecipeAsync(recipeId);
    
    // TODO: Fix Recipe Snapshot Value Object reference 
    public async Task CreateServiceExecutionAsync(string assignmentId, string requestId, string technicianId, string homeownerId, string propertyId, RecipeSnapshot recipeSnapshot, DateTime scheduledAt, bool isPriority)
        => await monitoringContextFacade.CreateServiceExecutionAsync(assignmentId, requestId, technicianId, homeownerId, propertyId, recipeSnapshot, scheduledAt, isPriority);
}