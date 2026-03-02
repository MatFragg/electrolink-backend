using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class CreateComponentCommandFromResourceAssembler
{
    public static CreateComponentCommand ToCommandFromResource(CreateComponentResource resource) 
        => new CreateComponentCommand(
            resource.Name, 
            resource.Description,
            resource.IsActive,
            ComponentTypeId.From(resource.ComponentTypeId)
        );
}

/*fasfsa*/