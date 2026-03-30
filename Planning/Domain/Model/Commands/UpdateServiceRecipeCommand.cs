using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record UpdateServiceRecipeCommand(
    CatalogId CatalogId,
    RecipeId RecipeId,
    TechnicianId TechnicianId,
    string? ServiceName,
    string? ServiceDescription,
    IReadOnlyList<ComponentRequirementItem>? ComponentRequirements,
    int? EstimatedDurationHours,
    int? EstimatedDurationMinutes,
    decimal? MaterialsEstimate,
    decimal? LaborCost,
    decimal? TotalPrice,
    string? Currency,
    IReadOnlyList<string>? Prerequisites,
    IReadOnlyList<string>? Deliverables,
    int? WarrantyMonths);