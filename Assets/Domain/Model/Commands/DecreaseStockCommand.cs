namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
public record DecreaseStockCommand(string TechnicianId, string ComponentId, int AmountToDecrease);
