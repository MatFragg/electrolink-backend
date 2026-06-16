namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record SendDeviceToMaintenanceCommand(
    string DeviceId,
    string Reason,
    DateTime? ExpectedReturnDate);
