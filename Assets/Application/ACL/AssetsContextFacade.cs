using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using TechnicianId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.TechnicianId;

namespace Hampcoders.Electrolink.API.Assets.Application.ACL;

public class AssetsContextFacade(ITechnicianInventoryCommandService technicianInventoryCommandService,
    ITechnicianInventoryQueryService technicianInventoryQueryService, IPropertyQueryService propertyQueryService) : IAssetsContextFacade
{
    public async Task<string> CreateTechnicianInventory(string technicianId)
    {
        var command = new CreateTechnicianInventoryCommand(TechnicianId.From(technicianId));
        var inventory = await technicianInventoryCommandService.Handle(command);
        return inventory!.TechnicianId.Value;
    }

    public async Task<bool> ExistsInventoryForTechnician(string technicianId)
    {
        var query = new GetInventoryByTechnicianIdQuery(TechnicianId.From(technicianId));
        var inventory = await technicianInventoryQueryService.Handle(query);
        return inventory != null;
    }
    
    public async Task<string?> FetchPropertyAddressAsync(string propertyId)
    {
        var query = new GetPropertyAddressQuery(PropertyId.From(propertyId));
        var address = await propertyQueryService.Handle(query); 
        return address.Street;
    }
    public async Task<bool> HasTechnicianEnoughStockAsync(string technicianId, string componentId, int requiredQuantity)
    {
        var inventoryQuery = new GetInventoryByTechnicianIdQuery(TechnicianId.From(technicianId));
        var inventory = await technicianInventoryQueryService.Handle(inventoryQuery);

        if (inventory == null) return false;

        var componentInStock = inventory.Inventory.StockItems.FirstOrDefault(c => c.ComponentId.Value == componentId);

        return componentInStock != null && componentInStock.QuantityAvailable >= requiredQuantity;
    }

    /// <summary>
    /// Ajusta el stock de componentes de un técnico.
    /// </summary>
    public async Task<bool> AdjustTechnicianStockAsync(string technicianId, List<(string ComponentId, int Quantity)> adjustments)
    {
        var command = new AdjustTechnicianInventoryCommand(TechnicianId.From(technicianId), adjustments.Select(a => new ComponentAdjustment(
            ComponentId.From(a.ComponentId), a.Quantity)).ToList());
        var result = await technicianInventoryCommandService.Handle(command);
        return result != null;
    }
}