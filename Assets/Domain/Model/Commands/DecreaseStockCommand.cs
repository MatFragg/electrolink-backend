namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record DecreaseStockCommand(Guid TechnicianId, Guid ComponentId, int AmountToDecrease);
