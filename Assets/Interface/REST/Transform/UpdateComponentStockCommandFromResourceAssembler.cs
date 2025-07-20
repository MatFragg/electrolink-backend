using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class UpdateComponentStockCommandFromResourceAssembler
{
    public static UpdateComponentStockCommand ToCommandFromResource(
        UpdateComponentStockResource resource, 
        Guid technicianId,
        Guid componentId)
    {
        return new UpdateComponentStockCommand(
            technicianId,
            componentId,
            resource.NewQuantity,
            resource.NewAlertThreshold);
    }

}