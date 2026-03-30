using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRequestRepository(AppDbContext context) : BaseRepository<ServiceRequest, RequestId>(context), IServiceRequestRepository
{

    public async Task<IEnumerable<ServiceRequest>> FindByHomeownerIdAsync(HomeownerId homeownerId)
    {
        return await Context.Set<ServiceRequest>()
            .Where(r => r.HomeownerId == homeownerId)
            .OrderByDescending(r => r.RequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceRequest>> FindPendingAssignmentAsync()
        => await Context.Set<ServiceRequest>()
            .Where(r => r.Status == ERequestStatus.PendingAssignment)
            .OrderByDescending(r => r.IsPriority)
            .ThenBy(r => r.CreatedDate)
            .ToListAsync();
}

