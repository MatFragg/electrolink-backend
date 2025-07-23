using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class UpdatePropertyCommandFromResourceAssembler
{
    public static UpdatePropertyCommand ToCommandFromResource(UpdatePropertyResource resource, Guid propertyId)
    {
        return new UpdatePropertyCommand(
            propertyId,
            resource.OwnerId,
            resource.Address,
            resource.RegionName,
            resource.RegionCode,
            resource.DistrictName,
            resource.DistrictUbigeo
        );
    }
}