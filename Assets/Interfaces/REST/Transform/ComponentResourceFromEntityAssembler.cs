using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ComponentResourceFromEntityAssembler
{
    public static ComponentResource ToResourceFromEntity(Component entity) 
        => new ComponentResource(
            entity.Id.Value,
            entity.Name,
            entity.Description,
            entity.IsActive
        );
}