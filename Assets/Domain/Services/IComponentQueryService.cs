using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IComponentQueryService
{
    Task<Component?> Handle(GetComponentByIdQuery query);
    Task<IEnumerable<Component>> Handle(GetAllComponentsQuery query);
    Task<IEnumerable<Component>> Handle(GetComponentsByTypeIdQuery query);
    Task<IEnumerable<Component>> Handle(GetComponentsByIdsQuery query);

}