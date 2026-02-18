namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ComponentRequirement
{
    public string ComponentTypeId { get; init; }
    public string ComponentTypeName { get; init; }
    public int Quantity { get; init; }
    public bool IsRequired { get; init; }
    
    public ComponentRequirement() : this(string.Empty, string.Empty, 0, true) { }
    
    public ComponentRequirement(string componentTypeId, string componentTypeName, int quantity, bool isRequired)
    {
        if (string.IsNullOrWhiteSpace(componentTypeId))
            throw new ArgumentException("Component type ID is required");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be > 0");
            
        ComponentTypeId = componentTypeId;
        ComponentTypeName = componentTypeName;
        Quantity = quantity;
        IsRequired = isRequired;
    }
}

