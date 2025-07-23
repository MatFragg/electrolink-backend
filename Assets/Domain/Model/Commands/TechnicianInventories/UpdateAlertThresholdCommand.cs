namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
public record UpdateAlertThresholdCommand(Guid TechnicianId, Guid ComponentId, int NewThreshold);
