namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Entities;

public class OrphanedFileDeletion
{
    public Guid Id { get; private set; }
    public string ProviderId { get; private set; } = string.Empty;
    public string Folder { get; private set; } = string.Empty;
    public string Reason { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public int RetryCount { get; private set; }
    public int MaxRetries { get; private set; } = 3;
    public string? LastError { get; private set; }

    private OrphanedFileDeletion() { }

    public static OrphanedFileDeletion Create(string providerId, string folder, string reason)
    {
        if (string.IsNullOrWhiteSpace(providerId))
            throw new ArgumentException("ProviderId cannot be empty.", nameof(providerId));
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Folder cannot be empty.", nameof(folder));

        return new OrphanedFileDeletion
        {
            Id = Guid.NewGuid(),
            ProviderId = providerId,
            Folder = folder,
            Reason = reason,
            CreatedAt = DateTime.UtcNow,
            RetryCount = 0
        };
    }

    public void IncrementRetry(string error)
    {
        RetryCount++;
        LastError = error;
    }

    public bool CanRetry() => RetryCount < MaxRetries;
}
