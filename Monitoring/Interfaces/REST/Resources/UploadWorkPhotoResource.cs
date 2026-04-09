namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record UploadWorkPhotoResource(
    string PhotoType,  
    string PhotoUrl,
    DateTime TakenAt,
    string? Notes
);