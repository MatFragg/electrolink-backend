using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class TechnicianInventory
{
    // --- PROPIEDADES ---
    public Guid Id { get; private set; }
    public TechnicianId TechnicianId { get; private set; }

    private readonly List<ComponentStock> _stockItems;
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
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
        
        _domainEvents.Add(new ComponentStockDecreasedEvent(
            stockItem.Id, 
            stockItem.ComponentId, 
            command.AmountToDecrease, 
            stockItem.QuantityAvailable, 
            DateTime.UtcNow));
        
        if (stockItem.QuantityAvailable <= stockItem.AlertThreshold)
        {
            _domainEvents.Add(new ComponentStockLowEvent(
                stockItem.Id, 
                stockItem.ComponentId, 
                stockItem.QuantityAvailable, 
                stockItem.AlertThreshold, 
                DateTime.UtcNow));
        }
    }
    
    public void Handle(IncreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");

        // La validación de la cantidad ahora está dentro del método IncreaseQuantity.
        stockItem.IncreaseQuantity(command.AmountToAdd);
        
        _domainEvents.Add(new ComponentStockIncreasedEvent(
            stockItem.Id, 
            stockItem.ComponentId, 
            command.AmountToAdd, 
            stockItem.QuantityAvailable, 
            DateTime.UtcNow));
    }
    
    public void Handle(UpdateComponentStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new ComponentId(command.ComponentId))
                        ?? throw new KeyNotFoundException($"Component with ID {command.ComponentId} not found in inventory.");

        int oldQuantity = stockItem.QuantityAvailable;
        int newQuantity = command.NewQuantity;
        bool quantityChanged = oldQuantity != newQuantity;

        if (quantityChanged)
        {
            int difference = newQuantity - oldQuantity;
            if (difference > 0)
            {
                stockItem.IncreaseQuantity(difference);
                _domainEvents.Add(new ComponentStockIncreasedEvent(
                    stockItem.Id,
                    stockItem.ComponentId,
                    difference,
                    stockItem.QuantityAvailable,
                    DateTime.UtcNow));
            }
            else
            {
                stockItem.DecreaseQuantity(-difference);
                _domainEvents.Add(new ComponentStockDecreasedEvent(
                    stockItem.Id,
                    stockItem.ComponentId,
                    -difference,
                    stockItem.QuantityAvailable,
                    DateTime.UtcNow));
            }
        }

        if (stockItem.AlertThreshold != command.NewAlertThreshold)
        {
            stockItem.UpdateAlertThreshold(command.NewAlertThreshold);
            _domainEvents.Add(new ComponentStockThresholdUpdatedEvent(
                stockItem.Id,
                stockItem.ComponentId,
                stockItem.AlertThreshold,
                DateTime.UtcNow));
        }

        if (stockItem.QuantityAvailable <= stockItem.AlertThreshold)
        {
            _domainEvents.Add(new ComponentStockLowEvent(
                stockItem.Id,
                stockItem.ComponentId,
                stockItem.QuantityAvailable,
                stockItem.AlertThreshold,
                DateTime.UtcNow));
        }
    }
    
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
  
    public void AdjustComponentQuantity(Guid componentId, int quantityAdjustment)
    {
        var componentIdValueObject = new ComponentId(componentId);
        var existingComponent = _stockItems.FirstOrDefault(c => c.ComponentId == componentIdValueObject);

        if (existingComponent != null)
        {
            // El componente ya existe, ajusta su cantidad.
            existingComponent.UpdateQuantity(quantityAdjustment);

            // Si la cantidad llega a 0 o menos, eliminar el componente del inventario.
            if (existingComponent.QuantityAvailable <= 0)
            {
                _stockItems.Remove(existingComponent);
            }
        }
        else
        {
            // El componente no existe, si el ajuste es positivo, añadirlo como un nuevo componente.
            if (quantityAdjustment > 0)
            {
                _stockItems.Add(new ComponentStock(componentIdValueObject, quantityAdjustment));
            }
            // Si el ajuste es negativo y el componente no existe, no hacemos nada (no se puede tener cantidad negativa).
        }
    }

    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}