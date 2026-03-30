namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;

public record ComponentStockDetailReadModel(
    string StockId,
    string ComponentId,
    string ComponentTypeId,
    string ComponentName,
    int QuantityAvailable,
    int AlertThreshold,
    DateTime LastUpdated
);

