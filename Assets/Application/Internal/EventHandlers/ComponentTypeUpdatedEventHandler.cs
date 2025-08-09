using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentTypeUpdatedEventHandler : IEventHandler<ComponentTypeUpdatedEvent>
{
    private readonly ILogger<ComponentTypeUpdatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentTypeUpdatedEventHandler(ILogger<ComponentTypeUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentTypeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: ComponentTypeUpdatedEvent recibido para Tipo de Componente: {notification.ComponentTypeId.Id}.");

        var integrationEvent = new ComponentTypeUpdatedIntegrationEvent(
            notification.ComponentTypeId,
            notification.NewName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentTypeUpdatedIntegrationEvent para Tipo de Componente: {notification.ComponentTypeId.Id}.");
    }
}