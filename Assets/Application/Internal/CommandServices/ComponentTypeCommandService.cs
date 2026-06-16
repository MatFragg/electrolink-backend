using Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;
using MediatR;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class ComponentTypeCommandService(IComponentTypeRepository componentTypeRepository, ITechnicianInventoryRepository inventoryRepository, IUnitOfWork unitOfWork, IMediator mediator,ILogger<ComponentTypeCommandService> logger) : IComponentTypeCommandService
{
    public async Task<ComponentType?> Handle(CreateComponentTypeCommand command)
    {
        if (await componentTypeRepository.ExistsByNameAsync(command.Name))
        {
            logger.LogWarning("[Assets BC] Intento de crear tipo de componente con nombre duplicado: {ComponentTypeName}.", command.Name);
            throw new DuplicateAssetException("ComponentType", $"name '{command.Name}'");
        }

        var componentType = ComponentType.Create(command.Name, command.Description);
        await componentTypeRepository.AddAsync(componentType);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in componentType.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        componentType.ClearDomainEvents();

        return componentType;
    }

    public async Task<ComponentType?> Handle(UpdateComponentTypeCommand command)
    {
        // Envuelve el 'int' en su Value Object antes de pasarlo al repositorio.
        var componentType = await componentTypeRepository.FindByIdAsync(command.ComponentTypeId);
        if (componentType is null) throw new AssetNotFoundException("ComponentType", command.ComponentTypeId.Value);

        componentType.Update(command);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in componentType.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        componentType.ClearDomainEvents();
        
        return componentType;
    }

    public async Task<bool> Handle(DeleteComponentTypeCommand command)
    {
        var componentType = await componentTypeRepository.FindByIdAsync(command.ComponentTypeId);
        if (componentType is null)
        {
            return false;
        }

        var hasStockItems = await inventoryRepository.ExistsStockItemsByComponentTypeId(componentType.Id);
        if (hasStockItems)
            throw new InvalidOperationException("Cannot delete a component type that is currently in use.");

        componentTypeRepository.Remove(componentType);
        await unitOfWork.CompleteAsync();
        
        return true; 
    }
    
    public async Task<ComponentType> Handle(ActivateComponentTypeCommand command)
    {
        var componentType = await componentTypeRepository.FindByIdAsync(command.ComponentTypeId);
        if (componentType is null) throw new AssetNotFoundException("ComponentType", command.ComponentTypeId.Value);

        componentType.Activate();
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in componentType.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        componentType.ClearDomainEvents();

        return componentType;
    }

    public async Task<ComponentType> Handle(DeactivateComponentTypeCommand command)
    {
        var componentType = await componentTypeRepository.FindByIdAsync(command.ComponentTypeId);
        if (componentType is null) throw new AssetNotFoundException("ComponentType", command.ComponentTypeId.Value);

        componentType.Deactivate();
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in componentType.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        componentType.ClearDomainEvents();

        return componentType; 
    }
}