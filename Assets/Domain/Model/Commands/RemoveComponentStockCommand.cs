namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RemoveComponentStockCommand(Guid TechnicianId, Guid ComponentId);