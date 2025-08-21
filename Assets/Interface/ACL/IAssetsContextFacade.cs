using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interface.ACL;

public interface IAssetsContextFacade
{
    Task<Guid> CreateTechnicianInventory(Guid technicianId);
    Task<bool> ExistsInventoryForTechnician(Guid technicianId);
    Task<Address?> FetchPropertyAddressAsync(Guid propertyId);
    Task<bool> HasTechnicianEnoughStockAsync(Guid technicianId, Guid componentId, int requiredQuantity);
    Task<bool> AdjustTechnicianStockAsync(Guid technicianId, List<(Guid ComponentId, int Quantity)> adjustments);

}