using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceAssignmentRepository(AppDbContext context) : BaseRepository<ServiceAssignment, AssignmentId>(context), IServiceAssignmentRepository
{
    public async Task<ServiceAssignment?> FindByRequestIdAsync(RequestId requestId)
        => await Context.Set<ServiceAssignment>()
            .Where(a => a.RequestId == requestId && a.Status == EAssignmentStatus.Failed)
            .OrderByDescending(a => a.CreatedDate)
            .FirstOrDefaultAsync();
}

