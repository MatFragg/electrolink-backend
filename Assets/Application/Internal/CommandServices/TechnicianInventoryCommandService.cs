using MediatR;
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
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(AddStockToInventoryCommand command)
    {
        // Validación extra
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.ComponentId));
        if (component is null) throw new ArgumentException($"Component with id {command.ComponentId} not found in catalog.");
    
        // Es CRUCIAL que FindByTechnicianIdAsync incluya StockItems (ya lo tienes implementado).
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        // El Aggregate Root es responsable de gestionar sus entidades internas.
        // Esto añade el ComponentStock a la colección _stockItems del inventario trackeado.
        inventory.Handle(command); 
    
        // CAMBIO CLAVE: EF Core detectará automáticamente la adición del ComponentStock
        // porque 'inventory' ya está trackeado y su colección _stockItems ha sido modificada.
        // NO se necesita inventoryRepository.Update(inventory); aquí.
        
        await unitOfWork.CompleteAsync(); // Esto guardará el nuevo ComponentStock y los cambios en el AR

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return inventory; // Retornar el inventario actualizado
    }

    public async Task<TechnicianInventory?> Handle(UpdateComponentStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");
        
        inventory.Handle(command);

        // CAMBIO CLAVE: Eliminar esta línea.
        // Si el AR solo modifica entidades hijas y no sus propias propiedades escalares,
        // esta llamada a Update(inventory) es redundante y puede causar el error.
        // EF Core detectará los cambios en las entidades hijas.
        // inventoryRepository.Update(inventory); 
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return inventory;
    }
    
    public async Task<bool> Handle(RemoveComponentStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command); 

        // CAMBIO CLAVE: Eliminar esta línea.
        // Similar al UpdateComponentStockCommand, si el AR solo modifica entidades hijas,
        // esta llamada a Update(inventory) es redundante.
        // inventoryRepository.Update(inventory);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return true; 
    }
    
    public async Task<TechnicianInventory?> Handle(IncreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId));
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.Handle(command);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
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
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return inventory;
    }
    
    public async Task<TechnicianInventory?> Handle(AdjustTechnicianInventoryCommand command)
    {
        var technicianId = new TechnicianId(command.TechnicianId);
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(technicianId);

        if (inventory is null)
        {
            // Console.WriteLine($"Error: Inventario no encontrado para el técnico con ID {command.TechnicianId}");
            return null; // El inventario no existe
        }

       
        foreach (var adjustment in command.Adjustments)
        {
            inventory.AdjustComponentQuantity(adjustment.ComponentId, adjustment.Quantity);
        }
        

        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in inventory.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        inventory.ClearDomainEvents();

        return inventory;
    }
}