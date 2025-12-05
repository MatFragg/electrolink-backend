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
    
    /// <inheritdoc/>
    public async Task<Plan?> FindByIdAsync(PlanId id)
    {
        return await Context.Set<Plan>()
            .FindAsync(id);
    }
    
    /*public new async Task<Plan?> FindByIdAsync(Guid id)
    {
        return await FindByIdAsync(new PlanId(id));
    }*/
    
    /// <inheritdoc/>
    public async Task<Plan?> FindDefaultAsync()
    {
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(p => p.IsDefault);
    }

    /// <inheritdoc/>
    public async Task<Plan?> FindDefaultPlanByRoleAsync(EUserRole role)
    {
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.IsDefault && (p.TargetRole == role || p.TargetRole == EUserRole.All));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Plan>> ListPlansByRoleAsync(EUserRole role)
    {
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .Where(p => p.TargetRole == role || p.TargetRole == EUserRole.All)
            .ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<Plan?> FindByStripePriceIdAsync(string stripePriceId)
    {
        // Asume que el agregado Plan tiene una propiedad pública llamada StripePriceId.
        // Si no la tiene, tendrás que añadirla al agregado Plan y a la configuración de EF Core.
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.StripePriceId == stripePriceId);
    }
}