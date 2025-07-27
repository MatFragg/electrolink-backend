using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentStockLowEventHandler : IEventHandler<ComponentStockLowEvent>
{
    private readonly ILogger<ComponentStockLowEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public ComponentStockLowEventHandler(ILogger<ComponentStockLowEventHandler> logger, IIntegrationEventPublisher integrationEventPublisher)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(ComponentStockLowEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"[Assets BC] Domain Event: ComponentStockLowEvent recibido para Componente: {notification.ComponentId.Id} con Cantidad: {notification.CurrentQuantity}.");

        // Publicar evento de integración para Service Design and Planning
        var integrationEvent = new ComponentStockLowIntegrationEvent(
            notification.ComponentId, // Usar el Value Object directamente si el IntegrationEvent lo permite, o .Id si el IntegrationEvent espera Guid
            notification.CurrentQuantity,
            notification.OccurredOn
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Assets BC] Publicado Integration Event: ComponentStockLowIntegrationEvent para Componente: {notification.ComponentId.Id}.");
    }
}