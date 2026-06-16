namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record PhotoResource(
    string PhotoId,
    string PhotoType,
    string PhotoUrl,
    string? ThumbnailUrl,
    string Format,
    long SizeBytes,
    DateTime TakenAt,
    string? Notes
);
