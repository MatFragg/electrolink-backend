using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;


public class ComponentQueryService(IComponentRepository componentRepository) : IComponentQueryService
{
    public async Task<Component?> Handle(GetComponentByIdQuery query)
        => await componentRepository.FindByIdAsync(ComponentId.From(query.ComponentId));

    public async Task<IEnumerable<Component>> Handle(GetComponentsByTypeIdQuery query) 
        => await componentRepository.FindByTypeIdAsync(ComponentTypeId.From(query.ComponentTypeId));
    

    public async Task<IEnumerable<Component>> Handle(GetAllComponentsQuery query) 
        => await componentRepository.ListAsync();

    public async Task<IEnumerable<Component>> Handle(GetComponentsByIdsQuery query)
    {
        var componentIds = query.ComponentIds.Select(id =>  ComponentId.From(id));
        return await componentRepository.FindByIdsAsync(componentIds);
    }
}