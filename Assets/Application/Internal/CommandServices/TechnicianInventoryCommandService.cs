using Cortex.Mediator;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class TechnicianInventoryCommandService(
    ITechnicianInventoryRepository inventoryRepository, 
    IComponentRepository componentRepository, // Necesario para validaciones
    IUnitOfWork unitOfWork, IMediator mediator, IIntegrationEventPublisher integrationEventPublisher)
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
        
        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(AddStockToInventoryCommand command)
    {
        // Validación extra
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.ComponentId));
        if (component is null) throw new ArgumentException($"Component with id {command.ComponentId} not found in catalog.");
    
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
    
        inventoryRepository.Update(inventory); 
        await unitOfWork.CompleteAsync();
    
        // Finalizar la transacción
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();
    
        // Recargar el inventario completo para tener los datos actualizados
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(UpdateComponentStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");
        
        inventory.Handle(command);

        inventoryRepository.Update(inventory);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return inventory;
    }
    
    public async Task<bool> Handle(RemoveComponentStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command); 

        inventoryRepository.Update(inventory); 
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return true;
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
        
        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();
        
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(DecreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.PublishAsync(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();
        
        return inventory;
    }
}