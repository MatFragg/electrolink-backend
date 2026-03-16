using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface ITechnicianInventoryRepository : IBaseRepository<TechnicianInventory, TechnicianInventoryId>
{
    Task<TechnicianInventory?> FindByTechnicianIdAsync(TechnicianId technicianId);
    Task<IEnumerable<ComponentStock>> FindStockItemsByTechnicianIdAsync(TechnicianId technicianId);
}