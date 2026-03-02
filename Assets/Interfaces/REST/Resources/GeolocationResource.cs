namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record GeolocationResource(decimal Latitude, decimal Longitude, int? Accuracy, string Source);
