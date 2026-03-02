namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record PropertyResource(
    string PropertyId,
    string HomeownerId,
    AddressResource Address,
    GeolocationResource Geolocation,
    string Status,
    bool IsActive
);