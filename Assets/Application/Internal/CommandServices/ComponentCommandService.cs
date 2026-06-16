using Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;
using MediatR;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class ComponentCommandService(IComponentRepository componentRepository, IComponentTypeRepository componentTypeRepository, IUnitOfWork unitOfWork, IMediator mediator, ILogger<ComponentCommandService> logger) : IComponentCommandService
{
    public async Task<Component?> Handle(CreateComponentCommand command)
    {
        if (await componentRepository.ExistsByNameAsync(command.Name))
            throw new DuplicateAssetException("Component", $"name '{command.Name}'");

        var component = Component.Create(command.Name, command.Description,command.IsActive); 
        await componentRepository.AddAsync(component);
        await unitOfWork.CompleteAsync();

        logger.LogInformation("[ComponentCommandService] Después de CompleteAsync. Número de eventos de dominio en el AR: {DomainEventCount}.", component.DomainEvents.Count);
        foreach (var domainEvent in component.DomainEvents)
        {
            logger.LogInformation("[ComponentCommandService] Publicando evento de dominio: {EventType} (ID: {EventId}).", domainEvent.GetType().Name, domainEvent.EventId);
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        component.ClearDomainEvents(); 

        return component;
    }

    public async Task<Component?> Handle(UpdateComponentCommand command)
    {
        var component = await componentRepository.FindByIdAsync(command.ComponentId); 
        if (component is null) throw new AssetNotFoundException("Component", command.ComponentId.Value);

        component.UpdateInfo(command); 
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in component.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        component.ClearDomainEvents();

        return component;
    }

    public async Task<bool> Handle(DeleteComponentCommand command)
    {
        var component = await componentRepository.FindByIdAsync(command.ComponentId); 
        if (component is null)
        {
            return false;
        }
        
        component.Deactivate(); 
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in component.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        component.ClearDomainEvents();

        return true;
    }
}
