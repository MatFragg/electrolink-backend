using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRequestRepository : BaseRepository<ServiceRequest, RequestId>, IServiceRequestRepository
{
    public ServiceRequestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ServiceRequest?> FindByIdAsync(RequestId requestId)
    {
        return await Context.Set<ServiceRequest>()
            .FirstOrDefaultAsync(r => r.Id == requestId);
    }

    public async Task<IEnumerable<ServiceRequest>> FindByHomeownerIdAsync(HomeownerId homeownerId)
    {
        return await Context.Set<ServiceRequest>()
            .Where(r => r.HomeownerId == homeownerId)
            .OrderByDescending(r => r.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceRequest>> FindPendingAssignmentsAsync()
    {
        return await Context.Set<ServiceRequest>()
            .Where(r => r.Status == RequestStatus.PendingAssignment)
            .ToListAsync();
    }

    public new async Task AddAsync(ServiceRequest request)
    {
        await Context.Set<ServiceRequest>().AddAsync(request);
    }

    public void Update(ServiceRequest request)
    {
        Context.Set<ServiceRequest>().Update(request);
    }
}

