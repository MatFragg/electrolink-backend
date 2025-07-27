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
    }

    public async Task Handle(ComponentCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: ComponentCreatedEvent recibido para Componente: {notification.ComponentId.Id}, Tipo: {notification.ComponentTypeId.Id}.");

        // Publicar evento de integración para un sistema de catálogo maestro
        var integrationEvent = new ComponentCreatedIntegrationEvent(
            notification.ComponentId,
            notification.ComponentTypeId,
            notification.ComponentName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentCreatedIntegrationEvent para Componente: {notification.ComponentId.Id}.");
    }
}
