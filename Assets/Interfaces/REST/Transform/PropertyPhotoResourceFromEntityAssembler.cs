using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class PropertyPhotoResourceFromEntityAssembler
{
    public static PropertyPhotoResource ToResource(PropertyPhoto photo, string? thumbnailUrl = null)
    {
        return new PropertyPhotoResource(
            photo.PublicUrl,
            photo.ProviderId,
            thumbnailUrl,
            photo.UploadedAt
        );
    }
}
