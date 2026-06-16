namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record ReinstallDeviceCommand(
    string DeviceId,
    string TechnicianId,
    string FirmwareVersion,
    DateTime ReinstalledAt);
