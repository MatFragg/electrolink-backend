namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource for updating the geolocation of a property.
/// </summary>
public record UpdateGeolocationResource(
    decimal Latitude,
    decimal Longitude,
    int? Accuracy,
    string Source);

