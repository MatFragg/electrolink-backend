namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
public record IncreaseStockCommand(string TechnicianId, string ComponentId, int AmountToAdd);
