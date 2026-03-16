using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class DecreaseStockCommandFromResourceAssembler
{
    public static DecreaseStockCommand ToCommandFromResource(AdjustStockAmountResource resource, string technicianId, string componentId)
        => new DecreaseStockCommand(
            TechnicianId.From(technicianId),
            ComponentId.From(componentId),
            resource.Amount);
}

