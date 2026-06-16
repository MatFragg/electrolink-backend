namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record PropertyPhoto
{
    public string PublicUrl { get; init; }
    public string ProviderId { get; init; }
    public DateTime UploadedAt { get; init; }

    private PropertyPhoto() { }

    public static PropertyPhoto Create(string publicUrl, string providerId)
        => new()
        {
            PublicUrl = publicUrl,
            ProviderId = providerId,
            UploadedAt = DateTime.UtcNow
        };
}
