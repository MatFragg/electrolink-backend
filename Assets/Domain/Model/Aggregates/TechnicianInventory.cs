using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class TechnicianInventory
{
    public Guid Id { get; private set; }
    public TechnicianId TechnicianId { get; private set; }
    private readonly List<ComponentStock> _stockItems;
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();
    private readonly List<IEvent> _domainEvents  = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents .AsReadOnly();
    
    private TechnicianInventory()
    {
        _stockItems = new List<ComponentStock>();
        TechnicianId = null!;
    }

    public TechnicianInventory(CreateTechnicianInventoryCommand command) : this()
    {
        Id = Guid.NewGuid();
        TechnicianId = new TechnicianId(command.TechnicianId);
    }

    private ComponentStock? GetStockItemByComponentId(ComponentId componentId)
    {
        return _stockItems.FirstOrDefault(s => s.ComponentId == componentId);
    }

    public void Handle(AddStockToInventoryCommand command)
    {
        var componentId = new ComponentId(command.ComponentId);
        if (GetStockItemByComponentId(componentId) != null)
        {
            throw new InvalidOperationException($"Stock for component {componentId} already exists.");
        }
        var newStockItem = new ComponentStock(this.Id, componentId, command.Quantity, command.AlertThreshold);
        _stockItems.Add(newStockItem);
    }
    public void Handle(DecreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");
        
        stockItem.DecreaseQuantity(command.AmountToDecrease);
    }
    
    public void Handle(IncreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");

        // La validación de la cantidad ahora está dentro del método IncreaseQuantity.
        stockItem.IncreaseQuantity(command.AmountToAdd);
    }
    
    
    public void Handle(UpdateComponentStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new ComponentId(command.ComponentId))
            ?? throw new KeyNotFoundException($"Component with ID {command.ComponentId} not found in inventory.");

        bool quantityChanged = stockItem.QuantityAvailable != command.NewQuantity;
        bool thresholdChanged = stockItem.AlertThreshold != command.NewAlertThreshold;

        // Lógica para aplicar los cambios a ComponentStock
        if (quantityChanged)
        {
            // Aquí deberías decidir si llamas a IncreaseQuantity/DecreaseQuantity
            // o si expones un método SetQuantity en ComponentStock si la lógica es simple.
            // Para este ejemplo, asumamos que ComponentStock tiene un método SetQuantity.
            // stockItem.SetQuantity(command.NewQuantity); // Si existe este método
            
            // O, si quieres usar los métodos existentes para registrar los eventos de cambio de cantidad:
            if (command.NewQuantity > stockItem.QuantityAvailable)
            {
                stockItem.IncreaseQuantity(command.NewQuantity - stockItem.QuantityAvailable);
                _domainEvents.Add(new ComponentStockIncreasedEvent(stockItem.Id, stockItem.ComponentId, command.NewQuantity - stockItem.QuantityAvailable, stockItem.QuantityAvailable, DateTime.UtcNow));
            }
            else if (command.NewQuantity < stockItem.QuantityAvailable)
            {
                stockItem.DecreaseQuantity(stockItem.QuantityAvailable - command.NewQuantity);
                _domainEvents.Add(new ComponentStockDecreasedEvent(stockItem.Id, stockItem.ComponentId, stockItem.QuantityAvailable - command.NewQuantity, stockItem.QuantityAvailable, DateTime.UtcNow));
            }
        }

        if (thresholdChanged)
        {
            stockItem.UpdateAlertThreshold(command.NewAlertThreshold);
            _domainEvents.Add(new ComponentStockThresholdUpdatedEvent(
                stockItem.Id,
                stockItem.ComponentId,
                stockItem.AlertThreshold,
                DateTime.UtcNow
            ));
        }

        // Si la cantidad final está por debajo del umbral, también emitir evento de stock bajo
        if (stockItem.QuantityAvailable <= stockItem.AlertThreshold)
        {
            _domainEvents.Add(new ComponentStockLowEvent(
                stockItem.Id,
                stockItem.ComponentId,
                stockItem.QuantityAvailable,
                stockItem.AlertThreshold,
                DateTime.UtcNow
            ));
        }
    }

    /// <summary>
    /// Maneja el comando para remover un ComponentStock del inventario.
    /// </summary>
    /// <param name="command">Comando con los IDs del técnico y componente.</param>
    /// <exception cref="KeyNotFoundException">Si el componente no se encuentra en el inventario.</exception>
    /// <exception cref="InvalidOperationException">Si el stock no puede ser removido (ej. cantidad > 0).</exception>
    public void Handle(RemoveComponentStockCommand command)
    {
        var componentId = new ComponentId(command.ComponentId);
        var stockItem = GetStockItemByComponentId(componentId);
        if (stockItem == null)
        {
            throw new KeyNotFoundException($"Component with ID {componentId.Id} not found in inventory.");
        }

        if (stockItem.QuantityAvailable > 0)
        {
            throw new InvalidOperationException($"Cannot remove component {componentId.Id} from inventory while stock is greater than 0.");
        }

        _stockItems.Remove(stockItem);
        // _domainEvents.Add(new ComponentStockRemovedEvent(stockItem.Id, stockItem.ComponentId, DateTime.UtcNow)); // Define este evento
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}