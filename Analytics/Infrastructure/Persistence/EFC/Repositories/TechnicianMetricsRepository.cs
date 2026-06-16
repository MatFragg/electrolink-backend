using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class TechnicianMetricsRepository(AppDbContext context)
    : BaseRepository<TechnicianMetrics, TechnicianMetricsId>(context), ITechnicianMetricsRepository
{
    public async Task<TechnicianMetrics?> FindByTechnicianAndPeriodAsync(TechnicianId technicianId, DateRange period)
    {
        return await Context.Set<TechnicianMetrics>()
            .FirstOrDefaultAsync(m =>
                m.TechnicianId.Value == technicianId.Value &&
                m.Period.Start == period.Start &&
                m.Period.End == period.End);
    }

    public async Task<TechnicianMetrics?> FindCurrentPeriodByTechnicianAsync(TechnicianId technicianId)
    {
        var currentPeriod = DateRange.CurrentMonth();
        return await Context.Set<TechnicianMetrics>()
            .FirstOrDefaultAsync(m =>
                m.TechnicianId.Value == technicianId.Value &&
                m.Period.Start == currentPeriod.Start);
    }
}
