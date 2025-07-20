namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record TechnicianInventoryResource(Guid TechnicianId, List<ComponentStockResource> StockItems);
