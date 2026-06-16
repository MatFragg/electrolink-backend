namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record WorkPhotoUploadUrlResource(
    string Url,
    string Signature,
    long Timestamp,
    string ApiKey
);
