namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateProfilePhotoCommand(
    string ProfileId,
    string UserId,
    string ProviderId,
    string PublicUrl,
    string Format,
    long SizeBytes
);
