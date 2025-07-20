using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class TechnicianInventoryCommandService(
    ITechnicianInventoryRepository inventoryRepository, 
    IComponentRepository componentRepository, // Necesario para validaciones
    IUnitOfWork unitOfWork)
    : ITechnicianInventoryCommandService
{
    public async Task<TechnicianInventory?> Handle(CreateTechnicianInventoryCommand command)
    {
        var technicianId = new TechnicianId(command.TechnicianId);
        var existingInventory = await inventoryRepository.FindByTechnicianIdAsync(technicianId);
        if (existingInventory is not null)
            throw new InvalidOperationException("An inventory for this technician already exists.");

        var inventory = new TechnicianInventory(command);
        await inventoryRepository.AddAsync(inventory);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(AddStockToInventoryCommand command)
    {
        // Validación extra
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.ComponentId));
        if (component is null) throw new ArgumentException($"Component with id {command.ComponentId} not found in catalog.");
    
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        // En lugar de usar inventory.Handle(command), crearemos directamente el ComponentStock
        var componentId = new ComponentId(command.ComponentId);
    
        // Verificar si ya existe el stock
        if (inventory.StockItems.Any(s => s.ComponentId == componentId)) // <-- CORREGIDO
        {
            throw new InvalidOperationException($"Stock for component {componentId.Id} already exists.");
        }
    
        // Crear nuevo stock item como una operación independiente
        var newStockItem = new ComponentStock(
            inventory.Id,
            componentId,
            command.Quantity,
            command.AlertThreshold);
    
        // Añadir directamente al repositorio (necesitas crear este método)
        await inventoryRepository.AddComponentStockAsync(newStockItem);
    
        // Finalizar la transacción
        await unitOfWork.CompleteAsync();
    
        // Recargar el inventario completo para tener los datos actualizados
        return await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
    }

    public async Task<TechnicianInventory?> Handle(UpdateComponentStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");
        
        var componentId = new ComponentId(command.ComponentId);
        var stockItem = inventory.StockItems.FirstOrDefault(s => s.ComponentId == componentId);
        if (stockItem == null) throw new KeyNotFoundException("Componente no encontrado en inventario.");
        
        await inventoryRepository.UpdateComponentStockAsync(
            stockItem.Id,
            command.NewQuantity,
            command.NewAlertThreshold);

        await unitOfWork.CompleteAsync();
        return await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
    }
    
    public async Task<bool> Handle(RemoveComponentStockCommand command)
    {
        var result = await inventoryRepository.RemoveComponentStockAsync(
            command.TechnicianId,
            command.ComponentId);

        if (result)
            await unitOfWork.CompleteAsync();

        return result;
    }
    
    /// <summary>
    /// Maneja la actualización del umbral de alerta para un item específico en el inventario.
    /// </summary>
    /// <summary>
    /// Maneja el comando para actualizar el umbral de alerta de un item.
    /// </summary>
    public async Task<TechnicianInventory?> Handle(IncreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(DecreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();
        return inventory;
    }
}