using Hampcoders.Electrolink.API.Shared.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
public record ServiceRecipeDetailResource(
    string RecipeId,
    string ServiceName,
    string ServiceDescription,
    string ServiceCategory,
    IReadOnlyList<ComponentRequirementResource> ComponentRequirements,
    int EstimatedDurationMinutes,
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string Currency,
    IReadOnlyList<string> Prerequisites,
    IReadOnlyList<string> Deliverables,
    int WarrantyMonths,
    bool IsActive,
    int TimesRequested);
