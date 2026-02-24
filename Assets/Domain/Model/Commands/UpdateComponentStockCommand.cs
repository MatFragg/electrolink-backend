namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentStockCommand(string TechnicianId,string ComponentId, int NewQuantity, int NewAlertThreshold);