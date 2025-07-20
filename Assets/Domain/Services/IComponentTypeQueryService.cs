using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IComponentTypeQueryService
{
    Task<ComponentType?> Handle(GetComponentTypeByIdQuery query);
    Task<IEnumerable<ComponentType>> Handle(GetAllComponentTypesQuery query);
}