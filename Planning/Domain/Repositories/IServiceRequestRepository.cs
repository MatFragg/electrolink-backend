using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceRequestRepository : IBaseRepository<ServiceRequest, RequestId>
{
    Task<IEnumerable<ServiceRequest>> FindByHomeownerIdAsync(HomeownerId homeownerId);
    Task<IEnumerable<ServiceRequest>> FindPendingAssignmentAsync();
}