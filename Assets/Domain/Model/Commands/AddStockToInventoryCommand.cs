namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddStockToInventoryCommand(Guid TechnicianId, Guid ComponentId, int Quantity, int AlertThreshold);
