namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
public record RemoveComponentStockCommand(Guid TechnicianId, Guid ComponentId);