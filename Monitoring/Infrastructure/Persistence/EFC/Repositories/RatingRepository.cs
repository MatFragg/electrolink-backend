using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EfCore;

public class RatingRepository(AppDbContext context)
    : BaseRepository<Rating>(context), IRatingRepository
{
    public async Task<Rating?> GetByRequestIdAsync(Guid requestId)
    {
        return await context.Set<Rating>()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
    }
    
    public async Task<IEnumerable<Rating>> GetByTechnicianIdAsync(int technicianId)
    {
        return await context.Set<Rating>()
            .Where(r => r.TechnicianId == technicianId)
            .ToListAsync();
    }
    
}