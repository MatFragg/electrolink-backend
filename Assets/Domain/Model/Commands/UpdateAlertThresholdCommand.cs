namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateAlertThresholdCommand(Guid TechnicianId, Guid ComponentId, int NewThreshold);
