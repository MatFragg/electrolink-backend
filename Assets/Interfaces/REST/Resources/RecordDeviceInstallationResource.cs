namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record RecordDeviceInstallationResource(
    string TechnicianId,
    string FirmwareVersion,
    DateTime InstalledAt);
