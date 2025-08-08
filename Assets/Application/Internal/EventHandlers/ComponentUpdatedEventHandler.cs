using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentUpdatedEventHandler : IEventHandler<ComponentUpdatedEvent>
{
    private readonly ILogger<ComponentUpdatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentUpdatedEventHandler(ILogger<ComponentUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: ComponentUpdatedEvent recibido para Componente: {notification.ComponentId.Id}.");

        var integrationEvent = new ComponentUpdatedIntegrationEvent(
            notification.ComponentId,
            notification.NewName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentUpdatedIntegrationEvent para Componente: {notification.ComponentId.Id}.");
    }
}