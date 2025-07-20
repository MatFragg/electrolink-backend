namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record ComponentStockResource(Guid ComponentId, string ComponentName, int QuantityAvailable, int AlertThreshold, DateTime LastUpdated);
