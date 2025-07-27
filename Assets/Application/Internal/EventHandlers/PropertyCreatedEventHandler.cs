using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class PropertyCreatedEventHandler : IEventHandler<PropertyCreatedEvent>
{
    private readonly ILogger<PropertyCreatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PropertyCreatedEventHandler(ILogger<PropertyCreatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PropertyCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: PropertyCreatedEvent recibido para Propiedad: {notification.PropertyId.Id}, Propietario: {notification.OwnerId.Id}.");

        // Publicar evento de integración para un sistema de gestión de clientes (CRM)
        var integrationEvent = new PropertyCreatedIntegrationEvent(
            notification.PropertyId,
            notification.OwnerId,
            notification.PropertyName,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: PropertyCreatedIntegrationEvent para Propiedad: {notification.PropertyId.Id}.");
    }
}