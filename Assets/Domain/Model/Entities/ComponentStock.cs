using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ComponentStock
{
    public Guid Id { get; private set; }
    public Guid TechnicianInventoryId { get; private set; }
    public ComponentId ComponentId { get; private set; }
    public int QuantityAvailable { get; private set; }
    public int AlertThreshold { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public TechnicianInventory TechnicianInventory { get; private set; } = null!;


    // Constructor para la creación de un nuevo item
    internal ComponentStock(Guid technicianInventoryId,ComponentId componentId, int quantity, int alertThreshold)
    {
        Id = Guid.NewGuid();
        TechnicianInventoryId = technicianInventoryId;
        ComponentId = componentId;
        QuantityAvailable = quantity;
        AlertThreshold = alertThreshold;
        LastUpdated = DateTime.UtcNow;
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
        ComponentId = new ComponentId(Guid.Empty);
    }
}