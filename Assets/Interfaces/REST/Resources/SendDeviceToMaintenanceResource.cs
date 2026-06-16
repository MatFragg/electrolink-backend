namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record SendDeviceToMaintenanceResource(
    string Reason,
    DateTime? ExpectedReturnDate);
