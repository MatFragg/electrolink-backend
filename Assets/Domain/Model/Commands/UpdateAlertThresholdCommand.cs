namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateAlertThresholdCommand(string TechnicianId, string ComponentId, int NewThreshold);
