using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface ITechnicianInventoryQueryService
{
    Task<TechnicianInventoryReadModel?> Handle(GetInventoryByTechnicianIdQuery query);
    Task<IEnumerable<ComponentStockDetailReadModel>> Handle(GetStockItemsByTechnicianIdQuery query);
}