using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.ACL;

public interface IAssetsContextFacade
{
    Task<string> CreateTechnicianInventory(string technicianId);
    Task<bool> ExistsInventoryForTechnician(string technicianId);
    Task<string?> FetchPropertyAddressAsync(string propertyId);
    Task<bool> HasTechnicianEnoughStockAsync(string technicianId, string componentId, int requiredQuantity);
    Task<bool> AdjustTechnicianStockAsync(string technicianId, List<(string ComponentId, int Quantity)> adjustments);

}