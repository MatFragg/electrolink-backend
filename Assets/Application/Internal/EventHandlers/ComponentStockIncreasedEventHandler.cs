using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentStockIncreasedEventHandler : IEventHandler<ComponentStockIncreasedEvent>
{
    private readonly ILogger<ComponentStockIncreasedEventHandler> _logger;

    public ComponentStockIncreasedEventHandler(ILogger<ComponentStockIncreasedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ComponentStockIncreasedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: Stock de Componente {notification.ComponentId.Id} aumentado en {notification.IncreasedQuantity}. Nuevo total: {notification.NewQuantityAvailable}.");
        // No necesariamente requiere un evento de integración a menos que haya un sistema de compras/logística externo.
        return Task.CompletedTask;
    }
}