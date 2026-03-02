namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record UpdateComponentStockResource(
    int NewQuantity, 
    int NewAlertThreshold
);