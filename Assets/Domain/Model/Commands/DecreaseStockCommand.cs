namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
public record DecreaseStockCommand(string TechnicianId, string ComponentId, int AmountToDecrease);
