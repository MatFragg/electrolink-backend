using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;

public class ExternalAssetService(IAssetsContextFacade assetsContextFacade)
{
    public async Task<string> CreateTechnicianInventoryAsync(string technicianId)
    {
        if (await assetsContextFacade.ExistsInventoryForTechnicianAsync(technicianId))
        {
            throw new InvalidOperationException("Technician inventory already exists.");
        }

        return await assetsContextFacade.CreateTechnicianInventoryAsync(technicianId);
    }
}  