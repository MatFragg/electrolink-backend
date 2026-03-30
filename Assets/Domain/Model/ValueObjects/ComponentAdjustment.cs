using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentAdjustment
{
    public ComponentId ComponentId { get; init; }
    public int Quantity { get; init; } 

    public ComponentAdjustment(ComponentId componentId, int quantity)
    {
        if (componentId.Value == string.Empty) 
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
