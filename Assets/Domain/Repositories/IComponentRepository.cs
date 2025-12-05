using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IComponentRepository :  IBaseRepository<Component, Guid>
{
    Task<Component?> FindByIdAsync(ComponentId id);
    Task<IEnumerable<Component>> FindByTypeIdAsync(ComponentTypeId typeId);
    Task<bool> ExistsByNameAsync(string name); // Útil para validaciones
    Task<IEnumerable<Component>> FindByIdsAsync(IEnumerable<ComponentId> ids);

}