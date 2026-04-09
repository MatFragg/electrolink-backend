using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Repository implementation for ServiceCancellationRequest aggregate.
/// </summary>
public class ServiceCancellationRequestRepository(AppDbContext context)
    : BaseRepository<ServiceCancellationRequest, CancellationRequestId>(context), IServiceCancellationRequestRepository
{
    /// <summary>
    /// Retrieves all cancellation requests for a specific service execution.
    /// </summary>
    public async Task<IEnumerable<ServiceCancellationRequest>> FindByExecutionIdAsync(ServiceExecutionId executionId)
    {
        return await Context.Set<ServiceCancellationRequest>()
            .Where(cr => cr.ExecutionId == executionId)
            .ToListAsync();
    }
}

