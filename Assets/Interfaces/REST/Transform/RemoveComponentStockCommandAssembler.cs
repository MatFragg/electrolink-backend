using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class RemoveComponentStockCommandAssembler
{
    public static RemoveComponentStockCommand ToCommandFromResource(string technicianId, string componentId)
    {
        return new RemoveComponentStockCommand(TechnicianId.From(technicianId), ComponentId.From(componentId));
    }
}