using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IComponentTypeRepository : IBaseRepository<ComponentType>
{
    Task<bool> ExistsByNameAsync(string name);
    Task<ComponentType?> FindByIdAsync(ComponentTypeId id);

}