using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record CreateServiceRecipeCommand(
    Guid CatalogId,
    Guid TechnicianId,
    string ServiceName,
    string ServiceDescription,
    ServiceCategory ServiceCategory,
    List<ComponentRequirementDto> ComponentRequirements,
    int EstimatedHours,
    int EstimatedMinutes,
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string Currency,
    int WarrantyValue,
    WarrantyUnit WarrantyUnit,
    List<string>? Prerequisites,
    List<string>? Deliverables
);

