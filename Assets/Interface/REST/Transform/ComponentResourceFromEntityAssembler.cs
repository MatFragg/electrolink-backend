using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class ComponentResourceFromEntityAssembler
{
    public static ComponentResource ToResourceFromEntity(Component entity)
    {
        return new ComponentResource(
            entity.Id.Id,
            entity.Name,
            entity.Description,
            entity.IsActive,
            entity.TypeId.Id
        );
    }
}