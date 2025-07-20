namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record IncreaseStockCommand(Guid TechnicianId, Guid ComponentId, int AmountToAdd);
