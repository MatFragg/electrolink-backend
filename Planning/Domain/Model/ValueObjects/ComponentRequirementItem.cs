using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public class ComponentRequirementItem
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string RecipeId { get; private set; } = string.Empty;
    public string ComponentTypeId { get; private set; }
    public string ComponentTypeName { get; private set; }
    public int Quantity { get; private set; }
    public bool IsRequired { get; private set; }

    protected ComponentRequirementItem() { }

    private ComponentRequirementItem(
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

    [JsonConstructor]
    public ComponentRequirementItem(
        string componentTypeId,
        string componentTypeName,
        int quantity,
        bool isRequired,
        string? id = null,
        string? recipeId = null)
        : this(componentTypeId, componentTypeName, quantity, isRequired)
    {
        if (!string.IsNullOrWhiteSpace(id))
            Id = id;
        if (!string.IsNullOrWhiteSpace(recipeId))
            RecipeId = recipeId;
    }

    public static ComponentRequirementItem Create(
        string componentTypeId,
        string componentTypeName,
        int quantity,
        bool isRequired)
        => new(componentTypeId, componentTypeName, quantity, isRequired);
}