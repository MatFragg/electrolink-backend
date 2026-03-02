using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class UpdateComponentStockCommandFromResourceAssembler
{
    public static UpdateComponentStockCommand ToCommandFromResource(
        UpdateComponentStockResource resource,
        string technicianId,
        string componentId)
    {
        if (resource is null) throw new ArgumentNullException(nameof(resource));

        return new UpdateComponentStockCommand(
            TechnicianId.From(technicianId),
            ComponentId.From(componentId),
            resource.NewQuantity,
            resource.NewAlertThreshold
        );
    }
}