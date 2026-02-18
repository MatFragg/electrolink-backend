using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceRequestRepository
{
    Task<ServiceRequest?> FindByIdAsync(RequestId requestId);
    Task<IEnumerable<ServiceRequest>> FindByHomeownerIdAsync(HomeownerId homeownerId);
    Task<IEnumerable<ServiceRequest>> FindPendingAssignmentsAsync();
    Task AddAsync(ServiceRequest request);
    void Update(ServiceRequest request);
}

