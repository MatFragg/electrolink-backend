namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record SignedUploadUrlResource(
    string Url,
    string Signature,
    long Timestamp
);
