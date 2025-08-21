using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.ACL;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Application.ACL;

public class AssetsContextFacade(ITechnicianInventoryCommandService technicianInventoryCommandService,
    ITechnicianInventoryQueryService technicianInventoryQueryService, IPropertyQueryService propertyQueryService) : IAssetsContextFacade
{
    public async Task<Guid> CreateTechnicianInventory(Guid technicianId)
    {
        var command = new CreateTechnicianInventoryCommand(technicianId);
        var inventory = await technicianInventoryCommandService.Handle(command);
        return inventory!.Id;
    }

    public async Task<bool> ExistsInventoryForTechnician(Guid technicianProfileId)
    {
        var query = new GetInventoryByTechnicianIdQuery(technicianProfileId);
        var inventory = await technicianInventoryQueryService.Handle(query);
        return inventory != null;
    }
    
    public async Task<Address?> FetchPropertyAddressAsync(Guid propertyId)
    {
        var query = new GetPropertyAddressQuery(propertyId);
        var address = await propertyQueryService.Handle(query); 
        return address;
    }
    public async Task<bool> HasTechnicianEnoughStockAsync(Guid technicianId, Guid componentId, int requiredQuantity)
    {
        var inventoryQuery = new GetInventoryByTechnicianIdQuery(technicianId);
        var inventory = await technicianInventoryQueryService.Handle(inventoryQuery);

        if (inventory == null) return false;

        var componentInStock = inventory.StockItems.FirstOrDefault(c => c.ComponentId.Id == componentId);

        return componentInStock != null && componentInStock.QuantityAvailable >= requiredQuantity;
    }

    /// <summary>
    /// Ajusta el stock de componentes de un técnico.
    /// </summary>
    public async Task<bool> AdjustTechnicianStockAsync(Guid technicianId, List<(Guid ComponentId, int Quantity)> adjustments)
    {
        var command = new AdjustTechnicianInventoryCommand(technicianId, adjustments.Select(a => new ComponentAdjustment(a.ComponentId, a.Quantity)).ToList());
        var result = await technicianInventoryCommandService.Handle(command);
        return result != null;
    }
}