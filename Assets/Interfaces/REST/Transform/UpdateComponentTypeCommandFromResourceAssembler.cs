using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class UpdateComponentTypeCommandFromResourceAssembler
{
    public static UpdateComponentTypeCommand ToCommandFromResource(UpdateComponentTypeResource resource, string typeId)
    {
        if (resource is null) throw new ArgumentNullException(nameof(resource));

        return new UpdateComponentTypeCommand(
            ComponentTypeId.From(typeId),
            resource.Name,
            resource.Description
        );
    }
}