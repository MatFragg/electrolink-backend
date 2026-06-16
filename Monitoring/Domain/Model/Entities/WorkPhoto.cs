using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class WorkPhoto
{
    public WorkPhotoId Id { get; private set; }
    public ServiceExecutionId ExecutionId { get; private set; }
    public EPhotoType PhotoType { get; private set; }
    public string PhotoUrl { get; private set; }
    public string ProviderId { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public long SizeBytes { get; private set; }
    public string Format { get; private set; }
    public DateTime TakenAt { get; private set; }
    public string? Notes { get; private set; }

    private WorkPhoto() { }

    public static WorkPhoto Create(
        ServiceExecutionId executionId,
        EPhotoType photoType,
        string photoUrl,
        string providerId,
        string? thumbnailUrl,
        long sizeBytes,
        string format,
        DateTime takenAt,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Photo URL cannot be empty.", nameof(photoUrl));
        if (string.IsNullOrWhiteSpace(providerId))
            throw new ArgumentException("ProviderId cannot be empty.", nameof(providerId));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Format cannot be empty.", nameof(format));
        if (sizeBytes <= 0)
            throw new ArgumentException("SizeBytes must be positive.", nameof(sizeBytes));

        return new WorkPhoto
        {
            Id = WorkPhotoId.NewId(),
            ExecutionId = executionId,
            PhotoType = photoType,
            PhotoUrl = photoUrl,
            ProviderId = providerId,
            ThumbnailUrl = thumbnailUrl,
            SizeBytes = sizeBytes,
            Format = format.ToLowerInvariant(),
            TakenAt = takenAt,
            Notes = notes,
        };
    }
}
