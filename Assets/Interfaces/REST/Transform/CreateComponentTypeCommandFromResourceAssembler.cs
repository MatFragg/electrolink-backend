using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class CreateComponentTypeCommandFromResourceAssembler
{
    public static CreateComponentTypeCommand ToCommandFromResource(CreateComponentTypeResource resource)
        => new CreateComponentTypeCommand(
            resource.Name, 
            resource.Description
        );
}