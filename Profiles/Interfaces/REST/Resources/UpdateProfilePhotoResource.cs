namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record UpdateProfilePhotoResource(
    string ProviderId,
    string PublicUrl,
    string Format,
    long SizeBytes
);
