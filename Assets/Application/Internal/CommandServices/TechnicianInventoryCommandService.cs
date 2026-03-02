using MediatR;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class TechnicianInventoryCommandService(
    ITechnicianInventoryRepository inventoryRepository, 
    IComponentRepository componentRepository,
    IUnitOfWork unitOfWork, IMediator mediator)
    : ITechnicianInventoryCommandService
{
    public async Task<TechnicianInventory?> Handle(CreateTechnicianInventoryCommand command)
    {
        if (await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId) is not null)
            throw new InvalidOperationException("An inventory for this technician already exists.");

        var inventory = TechnicianInventory.Create(command.TechnicianId);
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
        var component = await componentRepository.FindByIdAsync(command.ComponentId);
        if (component is null) throw new ArgumentException($"Component with id {command.ComponentId} not found in catalog.");
    
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);


        // El Aggregate Root es responsable de gestionar sus entidades internas.
        // Esto añade el ComponentStock a la colección _stockItems del inventario trackeado.
        inventory.AddStock(command.ComponentId, command.Quantity, command.AlertThreshold); 
    
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
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);

        
        inventory.UpdateStock(command.ComponentId, command.NewQuantity, command.NewAlertThreshold);

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
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);

        inventory.RemoveStock(command.ComponentId); 

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

    public async Task<TechnicianInventory?> Handle(ReserveComponentsForServiceCommand command)
    {
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);
        inventory.ReserveComponentsForService(command.ServiceId, command.ComponentsToReserve);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(ConsumeComponentsForServiceCommand command)
    {
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);
        inventory.ConsumeComponentsForService(command.ServiceId);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<bool> Handle(ReleaseReservationCommand command)
    {
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);
        inventory.ReleaseReservation(command.ServiceId, command.Reason);
        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<TechnicianInventory?> Handle(IncreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId);
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.IncreaseStock(command.ComponentId, command.AmountToAdd);
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
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId);
        if (inventory is null) throw new ArgumentException("Technician inventory not found.");

        inventory.DecreaseStock(command.ComponentId, command.AmountToDecrease);
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
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId);

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
    
    private async Task<TechnicianInventory> GetInventoryOrThrowAsync(TechnicianId technicianId)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(technicianId);
        return inventory ?? throw new KeyNotFoundException($"Inventory for technician {technicianId} not found.");
    }
}