namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record PauseDeviceStreamCommand(string DeviceId, string AdminId);
