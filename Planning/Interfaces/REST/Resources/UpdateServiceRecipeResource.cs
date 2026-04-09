using Hampcoders.Electrolink.API.Shared.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record UpdateServiceRecipeResource(
    string? ServiceName,
    string? ServiceDescription,
    IReadOnlyList<ComponentRequirementResource>? ComponentRequirements,
    int? EstimatedDurationHours,
    int? EstimatedDurationMinutes,
    PricingResource? Pricing,
    IReadOnlyList<string>? Prerequisites,
    IReadOnlyList<string>? Deliverables,
    int? WarrantyMonths);
