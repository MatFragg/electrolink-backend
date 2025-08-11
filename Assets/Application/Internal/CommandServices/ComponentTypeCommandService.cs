using MediatR;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class ComponentTypeCommandService(IComponentTypeRepository componentTypeRepository, IComponentRepository componentRepository, IUnitOfWork unitOfWork, IMediator mediator, IIntegrationEventPublisher integrationEventPublisher,ILogger<ComponentTypeCommandService> logger) : IComponentTypeCommandService
{
    public async Task<ComponentType?> Handle(CreateComponentTypeCommand command)
    {
        if (await componentTypeRepository.ExistsByNameAsync(command.Name))
        {
            logger.LogWarning($"[Assets BC] Intento de crear tipo de componente con nombre duplicado: {command.Name}.");
            throw new ArgumentException($"A component type with the name {command.Name} already exists.");
        }

        var componentType = new ComponentType(command);
        await componentTypeRepository.AddAsync(componentType);
        await unitOfWork.CompleteAsync();
        var createdEvent = new ComponentTypeCreatedEvent(
            componentType.Id,          
            componentType.Name,       
            DateTime.UtcNow
        );
        
        logger.LogInformation($"[Assets BC] Publicando evento de dominio ComponentTypeCreatedEvent (ID: {createdEvent.EventId}) para TipoComponenteId: {createdEvent.ComponentTypeId.Id}");
        await mediator.Publish(createdEvent, CancellationToken.None);

        componentType.ClearDomainEvents();
        return componentType;
    }

    public async Task<ComponentType?> Handle(UpdateComponentTypeCommand command)
    {
        // Envuelve el 'int' en su Value Object antes de pasarlo al repositorio.
        var componentTypeId = new ComponentTypeId(command.Id);
        var componentType = await componentTypeRepository.FindByIdAsync(componentTypeId);
        if (componentType is null) throw new ArgumentException("Component type not found.");

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
        var componentTypeId = new ComponentTypeId(command.Id);
        var componentType = await componentTypeRepository.FindByIdAsync(componentTypeId);
        if (componentType is null)
        {
            return false;
        }

        var componentsUsingType = await componentRepository.FindByTypeIdAsync(componentType.Id);
        if (componentsUsingType.Any())
            throw new InvalidOperationException("Cannot delete a component type that is currently in use.");

        componentTypeRepository.Remove(componentType);
        await unitOfWork.CompleteAsync();
        
        return true; 
    }
    
    
}