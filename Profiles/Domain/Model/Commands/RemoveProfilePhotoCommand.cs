namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record RemoveProfilePhotoCommand(
    string ProfileId,
    string UserId
);
