using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class CreatePropertyCommandFromResourceAssembler
{
    public static CreatePropertyCommand ToCommandFromResource(CreatePropertyResource resource, Guid ownerId)
    {
        var address = new Address(
            resource.Address.Street, 
            resource.Address.Number, 
            resource.Address.City, 
            resource.Address.PostalCode, 
            resource.Address.Country,
            resource.Address.Latitude,
            resource.Address.Longitude
        );

        var region = new Region(resource.RegionName);
        var district = new District(resource.DistrictName);

        return new CreatePropertyCommand(new OwnerId(ownerId), address, region, district);
    }
}