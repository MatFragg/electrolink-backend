using Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;
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
    IComponentTypeRepository componentTypeRepository,
    IUnitOfWork unitOfWork, IMediator mediator)
    : ITechnicianInventoryCommandService
{
    public async Task<TechnicianInventory?> Handle(CreateTechnicianInventoryCommand command)
    {
        if (await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId) is not null)
            throw new DuplicateAssetException("TechnicianInventory", $"technician '{command.TechnicianId}'");

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
        if (component is null) throw new AssetNotFoundException("Component", command.ComponentId.Value);
        
        var componentType = await componentTypeRepository.FindByIdAsync(command.ComponentTypeId);
        if (componentType is null) throw new AssetNotFoundException("ComponentType", command.ComponentTypeId.Value);
    
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);

        inventory.AddStock(command.ComponentId, command.ComponentTypeId, command.Quantity, command.AlertThreshold); 
        
        await unitOfWork.CompleteAsync(); 

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
        inventory.ReserveComponentsForService(command.AssignmentId, command.ComponentsToReserve);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<TechnicianInventory?> Handle(ConsumeComponentsForServiceCommand command)
    {
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);
        inventory.ConsumeComponentsForService(command.AssignmentId);
        await unitOfWork.CompleteAsync();
        return inventory;
    }

    public async Task<bool> Handle(ReleaseReservationCommand command)
    {
        var inventory = await GetInventoryOrThrowAsync(command.TechnicianId);
        inventory.ReleaseReservation(command.AssignmentId, command.Reason);
        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<TechnicianInventory?> Handle(IncreaseStockCommand command)
    {
        var inventory = await inventoryRepository.FindByTechnicianIdAsync(command.TechnicianId);
        if (inventory is null) throw new AssetNotFoundException("TechnicianInventory", command.TechnicianId.Value);

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
        if (inventory is null) throw new AssetNotFoundException("TechnicianInventory", command.TechnicianId.Value);

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
        return inventory ?? throw new AssetNotFoundException("TechnicianInventory", technicianId.Value);
    }
}