using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceRepository : IBaseRepository<Service, ServiceId>
{
    Task<IEnumerable<Service>> ListAllVisibleAsync();
    Task<IEnumerable<Service>> ListByCreatorAsync(TechnicianId creatorId);
    Task<IEnumerable<Service>> ListByCategoryAsync(string category);
}