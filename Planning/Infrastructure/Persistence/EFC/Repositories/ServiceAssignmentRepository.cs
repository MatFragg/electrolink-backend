using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceAssignmentRepository : BaseRepository<ServiceAssignment, ServiceId>, IServiceAssignmentRepository
{
    public ServiceAssignmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ServiceAssignment?> FindByIdAsync(ServiceId serviceId)
    {
        return await Context.Set<ServiceAssignment>()
            .FirstOrDefaultAsync(a => a.Id == serviceId);
    }

    public async Task<ServiceAssignment?> FindByRequestIdAsync(RequestId requestId)
    {
        return await Context.Set<ServiceAssignment>()
            .FirstOrDefaultAsync(a => a.RequestId == requestId);
    }

    public async Task<IEnumerable<ServiceAssignment>> FindByTechnicianIdAsync(TechnicianId technicianId)
    {
        return await Context.Set<ServiceAssignment>()
            .Where(a => a.TechnicianId == technicianId)
            .OrderByDescending(a => a.ScheduledSlot.StartDateTime)
            .ToListAsync();
    }

    public new async Task AddAsync(ServiceAssignment assignment)
    {
        await Context.Set<ServiceAssignment>().AddAsync(assignment);
    }

    public void Update(ServiceAssignment assignment)
    {
        Context.Set<ServiceAssignment>().Update(assignment);
    }
}

