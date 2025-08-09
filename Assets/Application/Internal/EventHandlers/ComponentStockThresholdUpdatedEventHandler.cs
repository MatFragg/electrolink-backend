using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentStockThresholdUpdatedEventHandler : IEventHandler<ComponentStockThresholdUpdatedEvent>
{
    private readonly ILogger<ComponentStockThresholdUpdatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentStockThresholdUpdatedEventHandler(ILogger<ComponentStockThresholdUpdatedEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentStockThresholdUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: Umbral de stock para Componente {notification.ComponentId.Id} actualizado a {notification.NewThreshold}.");
        var integrationEvent = new ComponentStockThresholdUpdatedIntegrationEvent(
            notification.ComponentId,
            notification.NewThreshold,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentStockThresholdUpdatedIntegrationEvent para Componente: {notification.ComponentId.Id}.");
    }
}