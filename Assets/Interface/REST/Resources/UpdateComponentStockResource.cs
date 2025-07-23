namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record UpdateComponentStockResource(
    int NewQuantity, 
    int? NewAlertThreshold
);