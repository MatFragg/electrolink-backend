using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRepository(AppDbContext context)
    : BaseRepository<Service, string>(context), IServiceRepository
{
    public async Task<IEnumerable<Service>> ListAllVisibleAsync()
    {
        return await context.Set<Service>()
            .Where(s => s.IsVisible)
            .ToListAsync();
    }
    
}