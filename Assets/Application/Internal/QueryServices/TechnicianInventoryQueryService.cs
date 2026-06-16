using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

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

    public async Task<IEnumerable<ComponentStockDetailReadModel>> Handle(GetStockItemsByTechnicianIdQuery query)
    {
        var stockItems = await technicianInventoryRepository.FindStockItemsByTechnicianIdAsync(query.TechnicianId);
        var stockList = stockItems.ToList();
        
        if (stockList.Count == 0) return Enumerable.Empty<ComponentStockDetailReadModel>();
        
        var componentIds = stockList.Select(s => s.ComponentId).Distinct();
        var components = await componentRepository.FindByIdsAsync(componentIds);
        var componentMap = components.ToDictionary(c => c.Id.Value, c => c.Name);

        return stockList.Select(item => new ComponentStockDetailReadModel(
            item.Id.Value,
            item.ComponentId.Value,
            item.ComponentTypeId.Value,
            componentMap.GetValueOrDefault(item.ComponentId.Value, "Unknown Component"),
            item.QuantityAvailable,
            item.AlertThreshold,
            item.LastUpdated
        ));
    }

    public async Task<bool> Handle(CheckComponentTypeStockQuery query)
    {
        var inventory = await technicianInventoryRepository
            .FindByTechnicianIdAsync(TechnicianId.From(query.TechnicianId));
    
        if (inventory is null) return false;

        return query.Requirements.All(req =>
        {
            var totalAvailable = inventory.StockItems
                .Where(s => s.ComponentTypeId.Value == req.ComponentTypeId)
                .Sum(s => s.AvailableForReservation);
            return totalAvailable >= req.Quantity;
        });
    }

    public async Task<IEnumerable<ComponentStockDetailReadModel>> Handle(GetComponentStockByComponentTypeIdAndTechnicianIdQuery query)
    {
        var stockItems = await technicianInventoryRepository
            .FindStockItemsByComponentTypeIdAndTechnicianIdAsync(query.TechnicianId, query.ComponentTypeId);
        var stockList = stockItems.ToList();
        if (stockList.Count == 0) return Enumerable.Empty<ComponentStockDetailReadModel>();

        var componentIds = stockList.Select(s => s.ComponentId).Distinct();
        var components = await componentRepository.FindByIdsAsync(componentIds);
        var componentMap = components.ToDictionary(c => c.Id.Value, c => c.Name);

        return stockList.Select(item => new ComponentStockDetailReadModel(
            item.Id.Value,
            item.ComponentId.Value,
            item.ComponentTypeId.Value,
            componentMap.GetValueOrDefault(item.ComponentId.Value, "Unknown Component"),
            item.QuantityAvailable,
            item.AlertThreshold,
            item.LastUpdated
        ));
    }
}