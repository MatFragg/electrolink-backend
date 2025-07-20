using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<IEnumerable<Service>> ListAllVisibleAsync();
    Task<Service?> FindByIdAsync(string serviceId);
}