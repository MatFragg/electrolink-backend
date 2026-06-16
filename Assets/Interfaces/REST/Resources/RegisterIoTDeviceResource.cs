namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record RegisterIoTDeviceResource(
    string SerialNumber,
    string PlainTextApiKey,
    string FirmwareVersion);
