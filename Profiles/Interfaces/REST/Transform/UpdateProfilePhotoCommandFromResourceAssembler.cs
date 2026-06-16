using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class UpdateProfilePhotoCommandFromResourceAssembler
{
    public static UpdateProfilePhotoCommand ToCommandFromResource(
        string profileId,
        string userId,
        UpdateProfilePhotoResource resource)
    {
        return new UpdateProfilePhotoCommand(
            profileId,
            userId,
            resource.ProviderId,
            resource.PublicUrl,
            resource.Format,
            resource.SizeBytes);
    }
}
