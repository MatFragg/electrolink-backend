namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record RegisterWorkPhotoResource(
    string ProviderId,
    string PublicUrl,
    string? ThumbnailUrl,
    string PhotoType,
    string Format,
    long SizeBytes,
    DateTime TakenAt,
    string? Notes
);
