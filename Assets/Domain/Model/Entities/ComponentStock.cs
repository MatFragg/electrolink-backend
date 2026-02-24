using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ComponentStock
{
    public ComponentStockId Id { get; private set; } = null!;
    public TechnicianInventoryId TechnicianInventoryId { get; private set; } = null!;
    public ComponentId ComponentId { get; private set; } = null!;
    public int QuantityAvailable { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int AlertThreshold { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public int AvailableForReservation => QuantityAvailable - ReservedQuantity;
    private ComponentStock() { }

    public static ComponentStock Create(TechnicianInventoryId technicianInventoryId,ComponentId componentId, int quantity, int alertThreshold)
    {
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        if (alertThreshold < 0) throw new ArgumentException("Alert threshold cannot be negative.");
        
        return new ComponentStock
        {
            Id = ComponentStockId.NewComponentStockId(),
            TechnicianInventoryId = technicianInventoryId,
            ComponentId = componentId,
            QuantityAvailable = quantity,
            AlertThreshold = alertThreshold,
            LastUpdated = DateTime.UtcNow
        };
    }
    
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
    
    internal void Reserve(int quantity)
    {
        if (AvailableForReservation < quantity)
            throw new InvalidOperationException("Insufficient available stock to reserve.");
        ReservedQuantity += quantity;
        LastUpdated       = DateTime.UtcNow;
    }
    
    internal void Release(int quantity)
    {
        ReservedQuantity = Math.Max(0, ReservedQuantity - quantity);
        LastUpdated      = DateTime.UtcNow;
    }

    internal void Consume(int quantity)
    {
        ReservedQuantity  = Math.Max(0, ReservedQuantity - quantity);
        QuantityAvailable = Math.Max(0, QuantityAvailable - quantity);
        LastUpdated       = DateTime.UtcNow;
    }

    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        QuantityAvailable = newQuantity;
        LastUpdated       = DateTime.UtcNow;
    }

    internal void UpdateAlertThreshold(int newThreshold)
    {
        if (newThreshold < 0) throw new ArgumentException("Threshold cannot be negative.");
        AlertThreshold = newThreshold;
        LastUpdated    = DateTime.UtcNow;
    }
}