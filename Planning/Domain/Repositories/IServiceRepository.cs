using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceRepository : IBaseRepository<Service, string>
{
    Task<IEnumerable<Service>> ListAllVisibleAsync();
}