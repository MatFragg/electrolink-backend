using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class AddStockToInventoryCommandFromResourceAssembler
{
    // Fíjate cómo este método recibe el ID del técnico desde la ruta del URL, además del recurso del body.
    public static AddStockToInventoryCommand ToCommandFromResource(AddStockToInventoryResource resource, Guid technicianId)
    {
        return new AddStockToInventoryCommand(technicianId, resource.ComponentId, resource.Quantity, resource.AlertThreshold);
    }
}