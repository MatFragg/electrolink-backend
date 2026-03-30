using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;


public class ComponentQueryService(IComponentRepository componentRepository) : IComponentQueryService
{
    public async Task<Component?> Handle(GetComponentByIdQuery query)
        => await componentRepository.FindByIdAsync(query.ComponentId);

    public async Task<IEnumerable<Component>> Handle(GetAllComponentsQuery query) 
        => await componentRepository.ListAsync();

    public async Task<IEnumerable<Component>> Handle(GetComponentsByIdsQuery query)
    {
        var componentIds = query.ComponentIds.Select(id => id);
        return await componentRepository.FindByIdsAsync(componentIds);
    }
}