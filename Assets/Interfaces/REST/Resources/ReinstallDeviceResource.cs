namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record ReinstallDeviceResource(
    string TechnicianId,
    string FirmwareVersion,
    DateTime ReinstalledAt);
