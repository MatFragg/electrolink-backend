using Hampcoders.Electrolink.API.Shared.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class OrphanedFileRepository : IOrphanedFileRepository
{
    private readonly AppDbContext _context;

    public OrphanedFileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OrphanedFileDeletion entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task<IReadOnlyList<OrphanedFileDeletion>> ListPendingAsync(int maxRetries)
    {
        return await _context.Set<OrphanedFileDeletion>()
            .Where(x => x.RetryCount < maxRetries)
            .OrderBy(x => x.CreatedAt)
            .Take(50)
            .ToListAsync();
    }

    public async Task MarkProcessedAsync(Guid id)
    {
        var entity = await _context.Set<OrphanedFileDeletion>().FindAsync(id);
        if (entity is not null)
        {
            _context.Remove(entity);
        }
    }
}
