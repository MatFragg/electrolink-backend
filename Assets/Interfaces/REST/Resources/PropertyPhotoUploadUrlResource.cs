namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record PropertyPhotoUploadUrlResource(
    string Url,
    string Signature,
    long Timestamp,
    string ApiKey
);
