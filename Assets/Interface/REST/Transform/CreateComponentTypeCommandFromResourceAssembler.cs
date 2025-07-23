using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class CreateComponentTypeCommandFromResourceAssembler
{
    public static CreateComponentTypeCommand ToCommandFromResource(CreateComponentTypeResource resource)
    {
        return new CreateComponentTypeCommand(resource.Name, resource.Description);
    }
}