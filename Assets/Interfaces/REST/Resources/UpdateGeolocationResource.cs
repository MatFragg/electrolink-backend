namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource for updating the geolocation of a property.
/// </summary>
public record UpdateGeolocationResource(
    double Latitude,
    double Longitude,
    int? Accuracy,
    string Source);

