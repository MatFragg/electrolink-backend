using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class TechnicianInventoryQueryService(ITechnicianInventoryRepository technicianInventoryRepository, IComponentRepository componentRepository) : ITechnicianInventoryQueryService
{
    public async Task<TechnicianInventoryReadModel?> Handle(GetInventoryByTechnicianIdQuery query)
    {
        var inventory = await technicianInventoryRepository.FindByTechnicianIdAsync(query.TechnicianId);
        if (inventory is null) return null;

        var componentIds = inventory.StockItems
            .Select(item => item.ComponentId)
            .ToList();

        var componentNames = componentIds.Any()
            ? (await componentRepository.FindByIdsAsync(componentIds))
            .ToDictionary(c => c.Id.Value, c => c.Name)
            : new Dictionary<string, string>();

        return new TechnicianInventoryReadModel(inventory, componentNames);
    }
}