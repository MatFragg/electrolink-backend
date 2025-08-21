namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentAdjustment
{
    public Guid ComponentId { get; init; }
    public int Quantity { get; init; } // Cantidad a ajustar (positiva para añadir, negativa para sustraer)

    public ComponentAdjustment(Guid componentId, int quantity)
    {
        if (componentId == Guid.Empty) 
            throw new ArgumentException("Component ID cannot be empty.", nameof(componentId));
        
        
        if (quantity == 0) 
            throw new ArgumentException("Quantity cannot be zero.", nameof(quantity));

        ComponentId = componentId;
        Quantity = quantity;
    }


    public ComponentAdjustment Invert() => new(ComponentId, -Quantity);
    public static ComponentAdjustment operator +(ComponentAdjustment a1, ComponentAdjustment a2)
    {
        if (a1.ComponentId != a2.ComponentId)
        {
            throw new InvalidOperationException("Cannot combine adjustments for different components.");
        }
        return new(a1.ComponentId, a1.Quantity + a2.Quantity);
    }
}
