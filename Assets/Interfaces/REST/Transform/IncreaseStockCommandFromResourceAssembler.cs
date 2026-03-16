using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class IncreaseStockCommandFromResourceAssembler
{
    public static IncreaseStockCommand ToCommandFromResource(AdjustStockAmountResource resource, string technicianId, string componentId)
        => new IncreaseStockCommand(
            TechnicianId.From(technicianId),
            ComponentId.From(componentId),
            resource.Amount);
}

