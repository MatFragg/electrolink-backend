namespace Hampcoders.Electrolink.API.Assets.Interface.ACL;

public interface IAssetsContextFacade
{
    Task<Guid> CreateTechnicianInventory(Guid technicianId);
    Task<bool> ExistsInventoryForTechnician(Guid technicianId);
}