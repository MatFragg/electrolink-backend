using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentTypeCreatedEventHandler : IEventHandler<ComponentTypeCreatedEvent>
{
    private readonly ILogger<ComponentTypeCreatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentTypeCreatedEventHandler(ILogger<ComponentTypeCreatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentTypeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: ComponentTypeCreatedEvent recibido para Tipo de Componente: {notification.ComponentTypeId.Id} - {notification.ComponentTypeName}.");

        var integrationEvent = new ComponentTypeCreatedIntegrationEvent(
            notification.ComponentTypeId,
            notification.ComponentTypeName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentTypeCreatedIntegrationEvent para Tipo de Componente: {notification.ComponentTypeId.Id}.");
    }
}