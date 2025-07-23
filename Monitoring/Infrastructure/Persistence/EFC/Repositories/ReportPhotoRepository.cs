using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;

public class ReportPhotoRepository(AppDbContext context)
    : BaseRepository<ReportPhoto>(context), IReportPhotoRepository
{
    public async Task<IEnumerable<ReportPhoto>> GetByReportIdAsync(Guid reportId)
    {
        return await context.Set<ReportPhoto>()
            .Where(p => p.ReportId == reportId)
            .ToListAsync();
    }
}