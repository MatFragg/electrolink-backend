using Hampcoders.Electrolink.API.Shared.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Shared.Domain.Repositories;

public interface IOrphanedFileRepository
{
    Task AddAsync(OrphanedFileDeletion entity);
    Task<IReadOnlyList<OrphanedFileDeletion>> ListPendingAsync(int maxRetries);
    Task MarkProcessedAsync(Guid id);
}
