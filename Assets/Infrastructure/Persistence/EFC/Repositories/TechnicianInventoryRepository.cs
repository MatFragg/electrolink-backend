using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class TechnicianInventoryRepository(AppDbContext context) : BaseRepository<TechnicianInventory>(context), ITechnicianInventoryRepository
{
    // El método principal para buscar un inventario
    public async Task<TechnicianInventory?> FindByTechnicianIdAsync(TechnicianId technicianId)
    {
        // Es CRUCIAL incluir las entidades hijas (StockItems) al cargar el agregado
        return await Context.Set<TechnicianInventory>()
            .Include(i => i.StockItems)
            .FirstOrDefaultAsync(i => i.TechnicianId == technicianId);
    }
    
    public Task AddComponentStockAsync(ComponentStock stockItem)
    {
        Context.Set<ComponentStock>().Add(stockItem);
        return Task.CompletedTask;
    }
    
    public async Task<bool> UpdateComponentStockAsync(Guid stockItemId, int newQuantity, int? newAlertThreshold)
    {
        var stock = await Context.Set<ComponentStock>().FindAsync(stockItemId);
        if (stock == null) return false;

        stock.UpdateQuantity(newQuantity);
        if (newAlertThreshold.HasValue)
            stock.UpdateAlertThreshold(newAlertThreshold.Value);

        return true;
    }

    public async Task<bool> RemoveComponentStockAsync(Guid technicianId, Guid componentId)
    {
        // 1. Creamos las instancias de los Value Objects para la consulta
        var technicianIdValueObject = new TechnicianId(technicianId);
        var componentIdValueObject = new ComponentId(componentId);

        // 2. CORRECCIÓN: Usamos Include() y navegamos a través de la relación
        var stockItem = await Context.Set<ComponentStock>()
            // Le decimos a EF Core que cargue la información del inventario padre (hace un JOIN)
            .Include(s => s.TechnicianInventory) 
            .FirstOrDefaultAsync(s => 
                // Comparamos el ID del técnico DENTRO del inventario relacionado
                s.TechnicianInventory.TechnicianId == technicianIdValueObject && 
                // Comparamos el ID del componente
                s.ComponentId == componentIdValueObject);

        if (stockItem == null) return false;

        Context.Set<ComponentStock>().Remove(stockItem);
        return true;
    }
}