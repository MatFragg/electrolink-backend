using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;

public class ReportRepository(AppDbContext context)
    : BaseRepository<Report, Guid>(context), IReportRepository
{
    public async Task<Report?> GetByRequestIdAsync(Guid requestId)
    {
        return await context.Set<Report>()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
    }
}