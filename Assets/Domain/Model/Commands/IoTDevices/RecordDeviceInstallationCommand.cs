namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RecordDeviceInstallationCommand(
    string DeviceId,
    string PropertyId,
    string TechnicianId,
    string? InstallationRequestId,
    string FirmwareVersion,
    DateTime InstalledAt);
