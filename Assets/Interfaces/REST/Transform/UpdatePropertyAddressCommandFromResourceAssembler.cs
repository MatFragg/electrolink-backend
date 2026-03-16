using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class UpdatePropertyAddressCommandFromResourceAssembler
{
    public static UpdatePropertyAddressCommand ToCommandFromResource(UpdateAddressResource resource, string propertyId)
        => new UpdatePropertyAddressCommand(PropertyId.From(propertyId), resource.NewAddress);
}

