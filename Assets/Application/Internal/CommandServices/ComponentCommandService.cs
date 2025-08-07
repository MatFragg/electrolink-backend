using MediatR;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Microsoft.Extensions.Logging; // Asegúrate de tener este using

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class ComponentCommandService : IComponentCommandService
{
    private readonly IComponentRepository componentRepository;
    private readonly IComponentTypeRepository componentTypeRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMediator mediator;
    private readonly IIntegrationEventPublisher integrationEventPublisher;
    private readonly ILogger<ComponentCommandService> logger;

    public ComponentCommandService(
        IComponentRepository componentRepository,
        IComponentTypeRepository componentTypeRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IIntegrationEventPublisher integrationEventPublisher,
        ILogger<ComponentCommandService> logger)
    {
        this.componentRepository = componentRepository;
        this.componentTypeRepository = componentTypeRepository;
        this.unitOfWork = unitOfWork;
        this.mediator = mediator;
        this.integrationEventPublisher = integrationEventPublisher;
        this.logger = logger;

        logger.LogInformation($"[ComponentCommandService CTOR] Tipo de IMediator inyectado: {mediator.GetType().FullName}");
    }

    public async Task<Component?> Handle(CreateComponentCommand command)
    {
        var componentType = await componentTypeRepository.FindByIdAsync(new ComponentTypeId(command.ComponentTypeId));
        if (componentType is null)
            throw new ArgumentException($"Component type with id {command.ComponentTypeId} not found.");

        if (await componentRepository.ExistsByNameAsync(command.Name))
            throw new ArgumentException($"A component with the name '{command.Name}' already exists.");

        var component = new Component(command); 
        await componentRepository.AddAsync(component);
        await unitOfWork.CompleteAsync();

        logger.LogInformation($"[ComponentCommandService] Después de CompleteAsync. Número de eventos de dominio en el AR: {component.DomainEvents.Count}");
        foreach (var domainEvent in component.DomainEvents)
        {
            logger.LogInformation($"[ComponentCommandService] Publicando evento de dominio: {domainEvent.GetType().Name} (ID: {domainEvent.EventId})");
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        component.ClearDomainEvents(); 

        return component;
    }

    public async Task<Component?> Handle(UpdateComponentCommand command)
    {
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.Id)); 
        if (component is null) throw new ArgumentException("Component not found.");

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
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.Id)); 
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
