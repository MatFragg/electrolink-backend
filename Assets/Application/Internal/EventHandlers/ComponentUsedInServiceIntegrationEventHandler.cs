using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class ComponentUsedInServiceIntegrationEventHandler : IEventHandler<ComponentUsedInServiceIntegrationEvent>
{
    private readonly ILogger<ComponentUsedInServiceIntegrationEventHandler> _logger;
    private readonly ITechnicianInventoryCommandService _technicianInventoryCommandService;

    public ComponentUsedInServiceIntegrationEventHandler(ILogger<ComponentUsedInServiceIntegrationEventHandler> logger, ITechnicianInventoryCommandService technicianInventoryCommandService)
    {
        _logger = logger;
        _technicianInventoryCommandService = technicianInventoryCommandService;
    }

    
    public async Task Handle(ComponentUsedInServiceIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Assets BC] Integration Event: ComponentUsedInServiceIntegrationEvent recibido para Servicio: {notification.ServiceId}, Técnico: {notification.TechnicianId.Id}, Componente: {notification.ComponentId.Id}, Cantidad: {notification.QuantityUsed}.");

        // Debes crear un objeto de comando y pasarlo al método Handle.
        var command = new DecreaseStockCommand(
            notification.TechnicianId.Id, 
            notification.ComponentId.Id, 
            notification.QuantityUsed
        );
        
        await _technicianInventoryCommandService.Handle(command);

        _logger.LogInformation($"[Assets BC] Inventario del Técnico {notification.TechnicianId.Id} actualizado para Componente {notification.ComponentId.Id}.");
    }
}
