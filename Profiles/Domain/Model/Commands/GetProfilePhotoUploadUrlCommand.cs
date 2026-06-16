namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record GetProfilePhotoUploadUrlCommand(
    string ProfileId,
    string UserId
);
