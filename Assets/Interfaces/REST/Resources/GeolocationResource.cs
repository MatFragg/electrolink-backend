namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record GeolocationResource(double Latitude, double Longitude, int? Accuracy, string Source);
