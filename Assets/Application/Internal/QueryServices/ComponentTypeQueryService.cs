using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class ComponentTypeQueryService(IComponentTypeRepository componentTypeRepository) : IComponentTypeQueryService
{
    public async Task<ComponentType?> Handle(GetComponentTypeByIdQuery query)
        => await componentTypeRepository.FindByIdAsync(query.ComponentTypeId);

    public async Task<IEnumerable<ComponentType>> Handle(GetAllComponentTypesQuery query)
        => await componentTypeRepository.ListAsync();

    public Task<string> Handle(GetComponentTypeNameByIdQuery query)
        => componentTypeRepository.FindComponentTypeNameByIdAsync(query.ComponentTypeId);
    
}