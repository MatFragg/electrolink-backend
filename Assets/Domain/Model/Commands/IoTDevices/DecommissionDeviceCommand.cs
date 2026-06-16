namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record DecommissionDeviceCommand(
    string DeviceId,
    string Reason);
