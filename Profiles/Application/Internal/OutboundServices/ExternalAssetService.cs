using Hampcoders.Electrolink.API.Assets.Interface.ACL;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;

public class ExternalAssetService(IAssetsContextFacade assetsContextFacade)
{
    public async Task<Guid> CreateTechnicianInventoryAsync(Guid technicianId)
    {
        if (await assetsContextFacade.ExistsInventoryForTechnician(technicianId))
        {
            throw new InvalidOperationException("Technician inventory already exists.");
        }

        return await assetsContextFacade.CreateTechnicianInventory(technicianId);
    }
}  