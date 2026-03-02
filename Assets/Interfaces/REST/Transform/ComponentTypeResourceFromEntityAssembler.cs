using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ComponentTypeResourceFromEntityAssembler
{
    public static ComponentTypeResource ToResourceFromEntity(ComponentType entity) 
        => new ComponentTypeResource(
            entity.Id.Value, 
            entity.Name, 
            entity.Description
        );
}