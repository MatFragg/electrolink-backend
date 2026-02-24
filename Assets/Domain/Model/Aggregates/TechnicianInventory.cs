using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class TechnicianInventory : BaseAggregateRoot
{
    // --- PROPIEDADES ---
    public TechnicianInventoryId Id { get; private set; }
    public TechnicianId TechnicianId { get; private set; }

    private readonly List<ComponentStock> _stockItems;
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();

    private TechnicianInventory()
    {
        _stockItems = new List<ComponentStock>();
        TechnicianId = null!;
    }

    public static TechnicianInventory Create(TechnicianId technicianId)
    {
        if (technicianId == null || technicianId.Value == string.Empty) throw new ArgumentException("Technician ID must be valid.", nameof(technicianId));
        
        var inventory = new TechnicianInventory
        {
            Id = TechnicianInventoryId.NewTechnicianInventoryId(),
            TechnicianId = technicianId
        };
        
        inventory.RaiseDomainEvent(new TechnicianInventoryCreatedEvent(inventory.Id, inventory.TechnicianId, DateTime.UtcNow));
        return inventory;
    }

    private ComponentStock? GetStockItemByComponentId(ComponentId componentId)
    {
        return _stockItems.FirstOrDefault(s => s.ComponentId == componentId);
    }

    public void Handle(AddStockToInventoryCommand command)
    {
        var componentId = ComponentId.From(command.ComponentId);
        if (GetStockItemByComponentId(componentId) != null)
        {
            throw new InvalidOperationException($"Stock for component {componentId} already exists.");
        }
        var newStockItem = ComponentStock.Create(Id, componentId, command.Quantity, command.AlertThreshold);
        _stockItems.Add(newStockItem);
    }
    public void Handle(DecreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(ComponentId.From(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");
        
        stockItem.DecreaseQuantity(command.AmountToDecrease);
        
        if (stockItem.QuantityAvailable <= stockItem.AlertThreshold)
        {
        }
    }
    
    public void Handle(IncreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(ComponentId.From(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");

        // La validación de la cantidad ahora está dentro del método IncreaseQuantity.
        stockItem.IncreaseQuantity(command.AmountToAdd);
    }
    
    public void Handle(UpdateComponentStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(ComponentId.From(command.ComponentId))
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
            }
            else
            {
                stockItem.DecreaseQuantity(-difference);
            }
        }

        if (stockItem.AlertThreshold != command.NewAlertThreshold)
        {
            stockItem.UpdateAlertThreshold(command.NewAlertThreshold);
        }

        if (stockItem.QuantityAvailable <= stockItem.AlertThreshold)
        {
        }
    }
    
    public void Handle(RemoveComponentStockCommand command)
    {
        var componentId = ComponentId.From(command.ComponentId);
        var stockItem = GetStockItemByComponentId(componentId);
        if (stockItem == null)
        {
            throw new KeyNotFoundException($"Component with ID {componentId.Value} not found in inventory.");
        }

        if (stockItem.QuantityAvailable > 0)
        {
            throw new InvalidOperationException($"Cannot remove component {componentId.Value} from inventory while stock is greater than 0.");
        }

        _stockItems.Remove(stockItem);
        // _domainEvents.Add(new ComponentStockRemovedEvent(stockItem.Id, stockItem.ComponentId, DateTime.UtcNow)); // Define este evento
    }
  
    public void AdjustComponentQuantity(string componentId, int quantityAdjustment)
    {
        var componentIdValueObject = ComponentId.From(componentId);
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
}