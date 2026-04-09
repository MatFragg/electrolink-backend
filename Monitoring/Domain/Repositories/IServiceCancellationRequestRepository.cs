using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Repositories;

public interface
    IServiceCancellationRequestRepository : IBaseRepository<ServiceCancellationRequest, CancellationRequestId>
{
    Task<IEnumerable<ServiceCancellationRequest>> FindByExecutionIdAsync(ServiceExecutionId executionId);
}