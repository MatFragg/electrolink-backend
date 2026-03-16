namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record ComponentStockResource(string ComponentStockId, string ComponentId, string ComponentName, int QuantityAvailable, int AlertThreshold, DateTime LastUpdated);
