using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ComponentRequirement
{
    public string ComponentTypeId { get; }
    public string ComponentTypeName { get; }
    public int Quantity { get; }
    public bool IsRequired { get; }

    private ComponentRequirement(
        string componentTypeId,
        string componentTypeName,
        int quantity,
        bool isRequired)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException(componentTypeId);

        ComponentTypeId   = componentTypeId;
        ComponentTypeName = componentTypeName;
        Quantity          = quantity;
        IsRequired        = isRequired;
    }

    public static ComponentRequirement Create(
        string componentTypeId,
        string componentTypeName,
        int quantity,
        bool isRequired)
        => new(componentTypeId, componentTypeName, quantity, isRequired);
}

