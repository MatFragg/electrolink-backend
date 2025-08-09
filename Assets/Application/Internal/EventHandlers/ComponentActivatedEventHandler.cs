using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentActivatedEventHandler : IEventHandler<ComponentActivatedEvent>
{
    private readonly ILogger<ComponentActivatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher; 

    public ComponentActivatedEventHandler(ILogger<ComponentActivatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentActivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: ComponentActivatedEvent recibido para Componente: {notification.ComponentId.Id}.");
        var integrationEvent = new ComponentStateChangedIntegrationEvent(
            notification.ComponentId,
            true,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentStateChangedIntegrationEvent para Componente: {notification.ComponentId.Id}.");
        return;
    }
}
