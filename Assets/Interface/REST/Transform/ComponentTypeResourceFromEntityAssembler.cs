using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class ComponentTypeResourceFromEntityAssembler
{
    public static ComponentTypeResource ToResourceFromEntity(ComponentType entity)
    {
        return new ComponentTypeResource(entity.Id.Id, entity.Name, entity.Description);
    }
}