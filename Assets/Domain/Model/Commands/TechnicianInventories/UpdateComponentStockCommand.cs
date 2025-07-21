namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentStockCommand(Guid TechnicianId,Guid ComponentId, int NewQuantity, int? NewAlertThreshold);