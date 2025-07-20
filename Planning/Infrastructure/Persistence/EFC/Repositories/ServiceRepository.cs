using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.API.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRepository(AppDbContext context)
    : BaseRepository<Service>(context), IServiceRepository
{
    public async Task<IEnumerable<Service>> ListAllVisibleAsync()
    {
        return await context.Set<Service>()
            .Where(s => s.IsVisible)
            .ToListAsync();
    }

    public async Task<Service?> FindByIdAsync(string serviceId)
    {
        return await context.Set<Service>()
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId);
    }
}