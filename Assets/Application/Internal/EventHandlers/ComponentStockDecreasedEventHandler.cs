using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentStockDecreasedEventHandler : IEventHandler<ComponentStockDecreasedEvent>
{
    private readonly ILogger<ComponentStockDecreasedEventHandler> _logger;

    public ComponentStockDecreasedEventHandler(ILogger<ComponentStockDecreasedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ComponentStockDecreasedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: Stock de Componente {notification.ComponentId.Id} disminuido en {notification.DecreasedQuantity}. Nuevo total: {notification.NewQuantityAvailable}.");
        // Aquí podrías actualizar un modelo de lectura, o realizar alguna otra acción interna.
        return Task.CompletedTask;
    }
}