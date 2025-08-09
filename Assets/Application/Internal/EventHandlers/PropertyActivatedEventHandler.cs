using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class PropertyActivatedEventHandler : IEventHandler<PropertyActivatedEvent>
{
    private readonly ILogger<PropertyActivatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PropertyActivatedEventHandler(ILogger<PropertyActivatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PropertyActivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: PropertyActivatedEvent recibido para Propiedad: {notification.PropertyId.Id}.");
        var integrationEvent = new PropertyStateChangedIntegrationEvent(
            notification.PropertyId,
            true, 
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        _logger.LogInformation($"[Assets BC] Publicado Integration Event: PropertyStateChangedIntegrationEvent para Propiedad: {notification.PropertyId.Id}.");
    }
}