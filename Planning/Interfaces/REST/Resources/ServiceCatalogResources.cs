namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

/// <summary>
/// Resource for creating a new service catalog
/// </summary>
public record CreateServiceCatalogResource(
    Guid TechnicianId
);

/// <summary>
/// Resource for creating a service recipe
/// </summary>
public record CreateServiceRecipeResource(
    Guid CatalogId,
    string ServiceName,
    string ServiceDescription,
    string ServiceCategory, // "SolarInstallation", "ElectricalMaintenance", "Repair", "Inspection", "Upgrade"
    List<ComponentRequirementResource> ComponentRequirements,
    int EstimatedHours,
    int EstimatedMinutes,
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string Currency,
    int WarrantyValue,
    string WarrantyUnit, // "Months" or "Years"
    List<string>? Prerequisites,
    List<string>? Deliverables
);

/// <summary>
/// Resource for updating a service recipe
/// </summary>
public record UpdateServiceRecipeResource(
    string ServiceName,
    string ServiceDescription,
    List<ComponentRequirementResource> ComponentRequirements,
    int EstimatedHours,
    int EstimatedMinutes,
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string Currency,
    int WarrantyValue,
    string WarrantyUnit,
    List<string>? Prerequisites,
    List<string>? Deliverables
);

/// <summary>
/// Component requirement nested resource
/// </summary>
public record ComponentRequirementResource(
    string ComponentTypeId,
    string ComponentTypeName,
    int Quantity,
    bool IsRequired
);

/// <summary>
/// Resource returned for service catalog
/// </summary>
public record ServiceCatalogResource(
    Guid CatalogId,
    Guid TechnicianId,
    string Status,
    List<ServiceRecipeResource> Recipes
);

/// <summary>
/// Resource returned for service recipe
/// </summary>
public record ServiceRecipeResource(
    Guid RecipeId,
    Guid CatalogId,
    string ServiceName,
    string ServiceDescription,
    string ServiceCategory,
    List<ComponentRequirementResource> ComponentRequirements,
    int EstimatedDurationMinutes,
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string Currency,
    int WarrantyValue,
    string WarrantyUnit,
    bool IsActive,
    int TimesRequested,
    List<string> Prerequisites,
    List<string> Deliverables
);

