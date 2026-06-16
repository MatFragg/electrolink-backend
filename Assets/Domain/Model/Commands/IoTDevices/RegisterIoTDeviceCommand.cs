namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RegisterIoTDeviceCommand(
    string SerialNumber,
    string PlainTextApiKey,
    string FirmwareVersion);
