namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddStockToInventoryCommand(string TechnicianId, string ComponentId, int Quantity, int AlertThreshold);
