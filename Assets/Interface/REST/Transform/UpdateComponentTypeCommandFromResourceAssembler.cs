using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class UpdateComponentTypeCommandFromResourceAssembler
{
    public static UpdateComponentTypeCommand ToCommandFromResource(UpdateComponentTypeResource resource, int typeId)
    {
        return new UpdateComponentTypeCommand(
            typeId,
            resource.Name,
            resource.Description
        );
    }
}