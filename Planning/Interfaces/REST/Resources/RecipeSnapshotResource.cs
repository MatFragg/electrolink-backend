using System.Collections.Generic;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record RecipeSnapshotResource(
    string RecipeId,
    string ServiceName,
    string ServiceCategory,
    List<ComponentRequirementResource> ComponentRequirements,
    decimal TotalPrice,
    string Currency,
    int EstimatedDurationMinutes,
    int WarrantyMonths,
    DateTime SnapshotAt
);

