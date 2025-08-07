using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;


public class ComponentCreatedEventHandler : IEventHandler<ComponentCreatedEvent>
{
    private readonly ILogger<ComponentCreatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentCreatedEventHandler(ILogger<ComponentCreatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        Console.WriteLine("[ComponentCreatedEventHandler CTOR] Instanciando ComponentCreatedEventHandler."); // <-- Nuevo log
    }

    public async Task Handle(ComponentCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[ComponentCreatedEventHandler HANDLE] INICIANDO manejo de ComponentCreatedEvent para Componente: {notification.ComponentId.Id}"); // <-- Nuevo log

        _logger.LogInformation($"[Assets BC] Domain Event: ComponentCreatedEvent recibido para Componente: {notification.ComponentId.Id}, Tipo: {notification.ComponentTypeId.Id}.");

        var integrationEvent = new ComponentCreatedIntegrationEvent(
            notification.ComponentId,
            notification.ComponentTypeId,
            notification.ComponentName,
            notification.OccurredOn,
            notification.EventId
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentCreatedIntegrationEvent para Componente: {notification.ComponentId.Id}.");
    }
}
