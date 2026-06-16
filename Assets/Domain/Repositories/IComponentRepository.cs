using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IComponentRepository :  IBaseRepository<Component, ComponentId>
{
    Task<bool> ExistsByNameAsync(string name); 
    Task<IEnumerable<Component>> FindByIdsAsync(IEnumerable<ComponentId> ids);

    Task<(IEnumerable<Component> Items, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize);
}