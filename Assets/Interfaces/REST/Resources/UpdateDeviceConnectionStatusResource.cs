using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record UpdateDeviceConnectionStatusResource(
    EConnectionStatus NewConnectionStatus,
    DateTime? LastReadingAt);
