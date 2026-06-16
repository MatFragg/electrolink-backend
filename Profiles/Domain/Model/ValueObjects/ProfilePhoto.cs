namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record ProfilePhoto
{
    public string PublicUrl { get; }
    public string ProviderId { get; }
    public DateTime UploadedAt { get; }

    private ProfilePhoto(string publicUrl, string providerId, DateTime uploadedAt)
    {
        PublicUrl = publicUrl;
        ProviderId = providerId;
        UploadedAt = uploadedAt;
    }

    public static ProfilePhoto Create(string publicUrl, string providerId)
    {
        if (string.IsNullOrWhiteSpace(publicUrl))
            throw new ArgumentException("PublicUrl cannot be empty.", nameof(publicUrl));
        if (string.IsNullOrWhiteSpace(providerId))
            throw new ArgumentException("ProviderId cannot be empty.", nameof(providerId));

        return new ProfilePhoto(publicUrl, providerId, DateTime.UtcNow);
    }
}
