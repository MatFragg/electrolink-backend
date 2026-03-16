using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class UpdatePropertyGeolocationCommandFromResourceAssembler
{
    public static UpdatePropertyGeolocationCommand ToCommandFromResource(UpdateGeolocationResource resource, string propertyId)
    {
        var geolocation = Geolocation.Create(resource.Latitude, resource.Longitude, resource.Accuracy, resource.Source);
        return new UpdatePropertyGeolocationCommand(PropertyId.From(propertyId), geolocation);
    }
}

