using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdatePropertyGeolocationCommand(PropertyId PropertyId, Geolocation NewGeolocation);