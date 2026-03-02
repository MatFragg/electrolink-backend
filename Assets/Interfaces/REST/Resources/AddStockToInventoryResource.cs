namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record AddStockToInventoryResource(string ComponentId, int Quantity, int AlertThreshold);
