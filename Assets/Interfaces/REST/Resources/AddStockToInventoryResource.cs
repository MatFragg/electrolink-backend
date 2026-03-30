namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record AddStockToInventoryResource(string ComponentId, string ComponentTypeId, int Quantity, int AlertThreshold);
