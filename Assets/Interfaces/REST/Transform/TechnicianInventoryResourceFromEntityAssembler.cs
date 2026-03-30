using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class TechnicianInventoryResourceFromEntityAssembler
{
    public static TechnicianInventoryResource ToResourceFromReadModel(TechnicianInventoryReadModel readModel)
    {
        var stockItems = readModel.Inventory.StockItems.Select(item => new ComponentStockResource(
            item.Id.Value,
            item.ComponentId.Value,
            item.ComponentTypeId.Value,
            readModel.ComponentNames.GetValueOrDefault(item.ComponentId.Value, "Unknown Component"),
            item.QuantityAvailable,
            item.AlertThreshold,
            item.LastUpdated
        )).ToList();

        return new TechnicianInventoryResource(readModel.Inventory.TechnicianId.Value, stockItems);
    }

    public static TechnicianInventoryResource ToResourceFromEntity(TechnicianInventory inventory, IDictionary<string, string>? componentNames = null)
    {
        var stockItems = inventory.StockItems.Select(item =>
        {
            var name = (componentNames != null && componentNames.TryGetValue(item.ComponentId.Value, out var n))
                ? n
                : "Unknown Component";
            
            return new ComponentStockResource(
                item.Id.Value,
                item.ComponentId.Value,
                item.ComponentTypeId.Value,
                name,
                item.QuantityAvailable,
                item.AlertThreshold,
                item.LastUpdated);
        }).ToList();
        return new TechnicianInventoryResource(inventory.TechnicianId.Value, stockItems);
    }
}