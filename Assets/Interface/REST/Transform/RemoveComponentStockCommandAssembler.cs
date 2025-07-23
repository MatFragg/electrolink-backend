using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class RemoveComponentStockCommandAssembler
{
    public static RemoveComponentStockCommand ToCommand(Guid technicianId, Guid componentId)
    {
        return new RemoveComponentStockCommand(technicianId, componentId);
    }
}