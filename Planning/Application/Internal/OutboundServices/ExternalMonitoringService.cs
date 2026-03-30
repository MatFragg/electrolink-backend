using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

public class ExternalMonitoringService(IMonitoringContextFacade monitoringContextFacade)
{
    public async Task<int> CountActiveServicesForRecipeAsync(string recipeId)
        => await monitoringContextFacade.CountActiveServicesForRecipeAsync(recipeId);

    public async Task<int> CountInProgressServicesForRecipeAsync(string recipeId)
        => await monitoringContextFacade.CountInProgressServicesForRecipeAsync(recipeId);
}