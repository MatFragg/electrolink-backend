using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ComponentStock
{
    public ComponentStockId Id { get; private set; }
    public TechnicianInventoryId TechnicianInventoryId { get; private set; }
    public ComponentId ComponentId { get; private set; } = null!;
    public int QuantityAvailable { get; private set; }
    public int AlertThreshold { get; private set; }
    public DateTime LastUpdated { get; private set; }
        
    public TechnicianInventory TechnicianInventory { get; private set; } = null!;


    // Constructor para la creación de un nuevo item
    public static ComponentStock Create(TechnicianInventoryId technicianInventoryId,ComponentId componentId, int quantity, int alertThreshold)
    {
        var stock = new ComponentStock
        {
            Id = ComponentStockId.NewComponentStockId(),
            TechnicianInventoryId = technicianInventoryId,
            ComponentId = componentId,
            QuantityAvailable = quantity,
            AlertThreshold = alertThreshold,
            LastUpdated = DateTime.UtcNow
        };
        
        return stock;
    }

    public ComponentStock(ComponentId componentId, int quantity)
    {
        ComponentId = componentId;
        QuantityAvailable = quantity;
    }

    // Métodos para modificar el estado
    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.");
        QuantityAvailable += amount;
        LastUpdated = DateTime.UtcNow;
    }

    internal void DecreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.");
        if (QuantityAvailable < amount) throw new InvalidOperationException("Insufficient stock.");
        QuantityAvailable -= amount;
        LastUpdated = DateTime.UtcNow;
    }

    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        QuantityAvailable = newQuantity;
        LastUpdated = DateTime.UtcNow;
    }

    internal void UpdateAlertThreshold(int newThreshold)
    {
        if (newThreshold < 0) throw new ArgumentException("Alert threshold cannot be negative.");
        AlertThreshold = newThreshold;
        LastUpdated = DateTime.UtcNow;
    }
    // Constructor privado para uso exclusivo de Entity Framework Core
    private ComponentStock() 
    {
    }
}