namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record AddStockToInventoryResource(Guid ComponentId, int Quantity, int AlertThreshold);
