namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record CreateServiceRecipeResource(
    string ServiceName,
    string ServiceDescription,
    string ServiceCategory,
    IReadOnlyList<ComponentRequirementResource> ComponentRequirements,
    int EstimatedDurationHours,
    int EstimatedDurationMinutes,
    PricingResource Pricing,
    IReadOnlyList<string> Prerequisites,
    IReadOnlyList<string> Deliverables,
    int WarrantyMonths);

