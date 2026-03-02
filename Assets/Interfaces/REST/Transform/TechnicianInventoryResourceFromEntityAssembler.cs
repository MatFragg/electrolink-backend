using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class TechnicianInventoryResourceFromEntityAssembler
{
    public static TechnicianInventoryResource ToResourceFromReadModel(TechnicianInventoryReadModel readModel)
    {
        var stockItems = readModel.Inventory.StockItems.Select(item => new ComponentStockResource(
            item.ComponentId.Value,
            readModel.ComponentNames.GetValueOrDefault(item.ComponentId.Value, "Unknown Component"),
            item.QuantityAvailable,
            item.AlertThreshold,
            item.LastUpdated
        )).ToList();

        return new TechnicianInventoryResource(readModel.Inventory.TechnicianId.Value, stockItems);
    }

    public static TechnicianInventoryResource ToResourceFromEntity(TechnicianInventory inventory)
    {
        var stockItems = inventory.StockItems.Select(item => new ComponentStockResource(
            item.ComponentId.Value,
            "Unknown Component", 
            item.QuantityAvailable,
            item.AlertThreshold,
            item.LastUpdated
        )).ToList();

        return new TechnicianInventoryResource(inventory.TechnicianId.Value, stockItems);
    }
}