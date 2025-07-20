using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;


public class ComponentQueryService(IComponentRepository componentRepository) : IComponentQueryService
{
    public async Task<Component?> Handle(GetComponentByIdQuery query)
    {
        return await componentRepository.FindByIdAsync(new (query.Id));
    }

    public async Task<IEnumerable<Component>> Handle(GetComponentsByTypeIdQuery query)
    {
        return await componentRepository.FindByTypeIdAsync(new (query.TypeId));
    }

    public async Task<IEnumerable<Component>> Handle(GetAllComponentsQuery query)
    {
        return await componentRepository.ListAsync();
    }

    // --- MÉTODO FALTANTE AÑADIDO ---
    public async Task<IEnumerable<Component>> Handle(GetComponentsByIdsQuery query)
    {
        // Convertimos la lista de Guids del query a una lista de Value Objects para el repositorio
        var componentIds = query.Ids.Select(id => new ComponentId(id));

        return await componentRepository.FindByIdsAsync(componentIds);
    }
}