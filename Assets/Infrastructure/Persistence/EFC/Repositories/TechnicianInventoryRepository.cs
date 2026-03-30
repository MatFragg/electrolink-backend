using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class TechnicianInventoryRepository(AppDbContext context) : BaseRepository<TechnicianInventory, TechnicianInventoryId>(context), ITechnicianInventoryRepository
{
    public async Task<TechnicianInventory?> FindByTechnicianIdAsync(TechnicianId technicianId) 
        => await Context.Set<TechnicianInventory>()
            .Include(i => i.StockItems)
            .FirstOrDefaultAsync(i => i.TechnicianId == technicianId);

    public async Task<IEnumerable<ComponentStock>> FindStockItemsByTechnicianIdAsync(TechnicianId technicianId) 
        => await Context.Set<TechnicianInventory>()
            .Where(i => i.TechnicianId == technicianId)
            .SelectMany(i => i.StockItems)
            .ToListAsync();

    public Task<bool> ExistsStockItemsByComponentTypeId(ComponentTypeId componentTypeId)
       => Context.Set<TechnicianInventory>()
           .SelectMany(i => i.StockItems)
           .AnyAsync(s => s.ComponentTypeId == componentTypeId);

    public async Task<IEnumerable<ComponentStock>> FindStockItemsByComponentTypeIdAndTechnicianIdAsync(TechnicianId technicianId, ComponentTypeId componentTypeId) 
        => await Context.Set<TechnicianInventory>()
            .Where(i => i.TechnicianId == technicianId)
            .SelectMany(i => i.StockItems)
            .Where(s => s.ComponentTypeId == componentTypeId)
            .ToListAsync();
}