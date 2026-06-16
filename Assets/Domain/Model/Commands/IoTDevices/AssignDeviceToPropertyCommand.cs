namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AssignDeviceToPropertyCommand(
    string DeviceId,
    string PropertyId,
    string InstallationRequestId);
