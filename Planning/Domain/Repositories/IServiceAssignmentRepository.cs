using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceAssignmentRepository
{
    Task<ServiceAssignment?> FindByIdAsync(ServiceId serviceId);
    Task<ServiceAssignment?> FindByRequestIdAsync(RequestId requestId);
    Task<IEnumerable<ServiceAssignment>> FindByTechnicianIdAsync(TechnicianId technicianId);
    Task AddAsync(ServiceAssignment assignment);
    void Update(ServiceAssignment assignment);
}

