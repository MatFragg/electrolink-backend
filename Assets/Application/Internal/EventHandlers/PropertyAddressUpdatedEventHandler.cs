using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class PropertyAddressUpdatedEventHandler : IEventHandler<PropertyAddressUpdatedEvent>
{
    private readonly ILogger<PropertyAddressUpdatedEventHandler> _logger;

    public PropertyAddressUpdatedEventHandler(ILogger<PropertyAddressUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PropertyAddressUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Domain Event: Dirección de Propiedad {notification.PropertyId.Id} actualizada a {notification.NewAddress}.");
        // Actualizar un modelo de lectura geoespacial, por ejemplo.
        return Task.CompletedTask;
    }
}