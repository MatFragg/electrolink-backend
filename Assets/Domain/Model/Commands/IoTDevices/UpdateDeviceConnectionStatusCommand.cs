using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateDeviceConnectionStatusCommand(
    string DeviceId,
    EConnectionStatus NewConnectionStatus,
    DateTime? LastReadingAt);
