using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class WorkPhoto
{
    public WorkPhotoId Id { get; private set; }
    public ServiceExecutionId ExecutionId { get; private set; }
    public EPhotoType PhotoType { get; private set; }
    public string PhotoUrl { get; private set; }
    public DateTime TakenAt { get; private set; }
    public string? Notes { get; private set; }

    private WorkPhoto() { }

    public static WorkPhoto Create(
        ServiceExecutionId executionId,
        EPhotoType photoType,
        string photoUrl,
        DateTime takenAt,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Photo URL cannot be empty.", nameof(photoUrl));

        return new WorkPhoto
        {
            Id = WorkPhotoId.NewId(),
            ExecutionId = executionId,
            PhotoType = photoType,
            PhotoUrl = photoUrl,
            TakenAt = takenAt,
            Notes = notes,
        };
    }
}
