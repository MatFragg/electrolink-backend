namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
public record IncreaseStockCommand(string TechnicianId, string ComponentId, int AmountToAdd);
