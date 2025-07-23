using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class CreateComponentCommandFromResourceAssembler
{
    public static CreateComponentCommand ToCommandFromResource(CreateComponentResource resource)
    {
        return new CreateComponentCommand(resource.Name, resource.Description,resource.ComponentTypeId);
    }
}