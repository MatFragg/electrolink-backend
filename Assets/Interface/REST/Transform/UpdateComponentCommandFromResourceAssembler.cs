using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class UpdateComponentCommandFromResourceAssembler
{
    public static UpdateComponentCommand ToCommandFromResource(UpdateComponentResource resource, Guid componentId)
    {
        return new UpdateComponentCommand(
            componentId, 
            resource.Name,
            resource.Description,
            resource.TypeId,
            resource.IsActive
        );
    }
}