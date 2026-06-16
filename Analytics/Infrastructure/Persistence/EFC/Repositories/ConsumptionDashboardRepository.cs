using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class ConsumptionDashboardRepository(AppDbContext context)
    : BaseRepository<ConsumptionDashboard, ConsumptionDashboardId>(context), IConsumptionDashboardRepository
{
    public async Task<ConsumptionDashboard?> FindByHomeownerIdAsync(HomeownerId homeownerId)
    {
        return await Context.Set<ConsumptionDashboard>()
            .FirstOrDefaultAsync(d => d.HomeownerId.Value == homeownerId.Value);
    }

    public async Task<List<ConsumptionDashboard>> FindAllActiveAsync()
    {
        return await Context.Set<ConsumptionDashboard>().ToListAsync();
    }
}
