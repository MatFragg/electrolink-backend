using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRepository(AppDbContext context)
    : BaseRepository<Service, ServiceId>(context), IServiceRepository
{
    public async Task<Service?> FindByIdAsync(ServiceId id)
    {
        return await Context.Set<Service>()
            .Include(s => s.Tags)
            .Include(s => s.Components)
            .Include(s => s.Plans)
            .Include(s => s.Documents)
            .Include(s => s.Policy)
            .Include(s => s.Restriction)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Service>> ListAllVisibleAsync()
    {
        return await Context.Set<Service>()
            .Include(s => s.Tags)
            .Include(s => s.Components)
            .Include(s => s.Plans)
            .Include(s => s.Documents)
            .Include(s => s.Policy)
            .Include(s => s.Restriction)
            .Where(s => s.IsVisible)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> ListByCreatorAsync(TechnicianId creatorId)
    {
        if (creatorId == null) return Enumerable.Empty<Service>();

        return await Context.Set<Service>()
            .Include(s => s.Tags)
            .Include(s => s.Components)
            .Include(s => s.Plans)
            .Include(s => s.Documents)
            .Include(s => s.Policy)
            .Include(s => s.Restriction)
            .Where(s => s.CreatedBy == creatorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> ListByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category)) return Enumerable.Empty<Service>();

        return await Context.Set<Service>()
            .Include(s => s.Tags)
            .Include(s => s.Components)
            .Include(s => s.Plans)
            .Include(s => s.Documents)
            .Include(s => s.Policy)
            .Include(s => s.Restriction)
            .Where(s => s.Category == category)
            .ToListAsync();
    }
}