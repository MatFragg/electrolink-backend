namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record PropertyPhotoResource(
    string PublicUrl,
    string ProviderId,
    string? ThumbnailUrl,
    DateTime UploadedAt
);
