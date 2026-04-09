using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record RecipeSnapshot
{
    public RecipeId RecipeId { get; }
    public string ServiceName { get; }
    public EServiceCategory ServiceCategory { get; }
    public IReadOnlyList<ComponentRequirementItem> ComponentRequirements { get; }
    public ServicePricing Pricing { get; }
    public EstimatedDuration EstimatedDuration { get; }
    public WarrantyPeriod WarrantyPeriod { get; }
    public DateTime SnapshotAt { get; }

    [JsonConstructor]
    private RecipeSnapshot(
        RecipeId recipeId,
        string serviceName,
        EServiceCategory serviceCategory,
        IReadOnlyList<ComponentRequirementItem> componentRequirements,
        ServicePricing pricing,
        EstimatedDuration estimatedDuration,
        WarrantyPeriod warrantyPeriod,
        DateTime snapshotAt)
    {
        RecipeId = recipeId;
        ServiceName = serviceName;
        ServiceCategory = serviceCategory;
        ComponentRequirements = componentRequirements;
        Pricing = pricing;
        EstimatedDuration = estimatedDuration;
        WarrantyPeriod = warrantyPeriod;
        SnapshotAt = snapshotAt;
    }

    /// <summary>
    /// Factory method para crear un snapshot a partir de una ServiceRecipe.
    /// Hotspot 2 & 7: Captura inmutable del recipe al momento de la asignación.
    /// Garantiza que ni cambios posteriores ni desactivaciones afecten servicios ya asignados.
    /// </summary>
    public static RecipeSnapshot FromRecipe(ServiceRecipe recipe)
        => new(
            recipe.Id,
            recipe.ServiceName,
            recipe.ServiceCategory,
            recipe.ComponentRequirements.ToList().AsReadOnly(),
            recipe.Pricing,
            recipe.EstimatedDuration,
            recipe.WarrantyPeriod,
            DateTime.UtcNow);

    /// <summary>
    /// Factory method para crear un snapshot manualmente.
    /// </summary>
    public static RecipeSnapshot Create(
        RecipeId recipeId,
        string serviceName,
        EServiceCategory serviceCategory,
        IEnumerable<ComponentRequirementItem> componentRequirements,
        ServicePricing pricing,
        EstimatedDuration estimatedDuration,
        WarrantyPeriod warrantyPeriod,
        DateTime snapshotAt)
        => new(
            recipeId,
            serviceName,
            serviceCategory,
            componentRequirements.ToList().AsReadOnly(),
            pricing,
            estimatedDuration,
            warrantyPeriod,
            snapshotAt);
}
