using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class AddStockToInventoryCommandFromResourceAssembler
{
    public static AddStockToInventoryCommand ToCommandFromResource(AddStockToInventoryResource resource, string technicianId) 
        => new AddStockToInventoryCommand(
            TechnicianId.From(technicianId), 
            ComponentId.From(resource.ComponentId), 
            resource.Quantity, 
            resource.AlertThreshold
        );
}