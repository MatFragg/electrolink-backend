using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class PropertyDetailsUpdatedEventHandler : IEventHandler<PropertyDetailsUpdatedEvent>
{
    private readonly ILogger<PropertyDetailsUpdatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PropertyDetailsUpdatedEventHandler(ILogger<PropertyDetailsUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PropertyDetailsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: PropertyDetailsUpdatedEvent recibido para Propiedad: {notification.Id}.");
        var propertyName = $"{notification.Address}, {notification.District}, {notification.Region}";

        var integrationEvent = new PropertyDetailsUpdatedIntegrationEvent(
            notification.Id,
            propertyName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        _logger.LogInformation($"[Assets BC] Publicado Integration Event: PropertyDetailsUpdatedIntegrationEvent para Propiedad: {notification.Id}.");
    }
}