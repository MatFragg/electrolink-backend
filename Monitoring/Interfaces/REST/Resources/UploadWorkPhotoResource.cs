namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record UploadWorkPhotoResource(
    string PhotoType,
    string PhotoUrl,
    string ProviderId,
    string? ThumbnailUrl,
    long SizeBytes,
    string Format,
    DateTime TakenAt,
    string? Notes
);
