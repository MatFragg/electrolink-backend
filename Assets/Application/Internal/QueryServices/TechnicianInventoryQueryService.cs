using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class TechnicianInventoryQueryService(ITechnicianInventoryRepository technicianInventoryRepository) : ITechnicianInventoryQueryService
{
    public async Task<TechnicianInventory?> Handle(GetInventoryByTechnicianIdQuery query)
    {
        return await technicianInventoryRepository.FindByTechnicianIdAsync(new (query.TechnicianId));
    }
}