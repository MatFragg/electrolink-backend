using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EfCore;

public class ServiceOperationRepository(AppDbContext context)
    : BaseRepository<ServiceOperation>(context), IServiceOperationRepository
{
    public async Task<ServiceOperation?> GetByIdAsync(Guid requestId)
    {
        return await context.Set<ServiceOperation>()
            .Include(s => s.StatusHistory)
            .FirstOrDefaultAsync(s => s.RequestId == requestId);
    }
    

    public async Task<IEnumerable<ServiceOperation>> GetByTechnicianIdAsync(int technicianId)
    {
        return (await context.Set<ServiceOperation>()
                .Include(s => s.StatusHistory)
                .ToListAsync()) 
            .Where(s => s.TechnicianId.Value == technicianId); 
    }
    
}

