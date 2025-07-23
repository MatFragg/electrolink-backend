namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
public record DecreaseStockCommand(Guid TechnicianId, Guid ComponentId, int AmountToDecrease);
