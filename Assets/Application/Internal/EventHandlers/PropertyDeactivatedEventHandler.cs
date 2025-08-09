using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class PropertyDeactivatedEventHandler : IEventHandler<PropertyDeactivatedEvent>
{
    private readonly ILogger<PropertyDeactivatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PropertyDeactivatedEventHandler(ILogger<PropertyDeactivatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PropertyDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: PropertyDeactivatedEvent recibido para Propiedad: {notification.PropertyId.Id}.");
        var integrationEvent = new PropertyStateChangedIntegrationEvent(
            notification.PropertyId,
            false, 
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        _logger.LogInformation($"[Assets BC] Publicado Integration Event: PropertyStateChangedIntegrationEvent para Propiedad: {notification.PropertyId.Id}.");
    }
}