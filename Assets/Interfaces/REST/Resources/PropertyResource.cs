using System.Collections.Generic;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record PropertyResource(
    string PropertyId,
    string HomeownerId,
    AddressResource Address,
    GeolocationResource Geolocation,
    string Status,
    bool IsActive,
    string? MainPhotoProviderId,
    IEnumerable<PropertyPhotoResource> Photos
);