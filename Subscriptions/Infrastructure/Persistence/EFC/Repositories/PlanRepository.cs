using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class PlanRepository(AppDbContext context)
    : BaseRepository<Plan>(context), IPlanRepository
{
    public async Task<Plan?> FindByIdAsync(PlanId id)
    {
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Plan?> FindDefaultAsync()
    {
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(p => p.IsDefault);
    }
    
}