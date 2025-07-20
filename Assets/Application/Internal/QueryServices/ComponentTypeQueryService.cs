using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class ComponentTypeQueryService(IComponentTypeRepository componentTypeRepository) : IComponentTypeQueryService
{
    public async Task<ComponentType?> Handle(GetComponentTypeByIdQuery query)
    {
        var componentTypeId = new ComponentTypeId(query.Id);
        return await componentTypeRepository.FindByIdAsync(componentTypeId);
    }

    public async Task<IEnumerable<ComponentType>> Handle(GetAllComponentTypesQuery query)
    {
        return await componentTypeRepository.ListAsync();
    }
}