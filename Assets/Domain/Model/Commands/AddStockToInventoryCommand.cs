namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;

public record AddStockToInventoryCommand(string TechnicianId, string ComponentId, int Quantity, int AlertThreshold);
