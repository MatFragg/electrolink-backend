namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;

public record StockItemReadModel(
    string ComponentId,
    string ComponentName,
    int Quantity
);