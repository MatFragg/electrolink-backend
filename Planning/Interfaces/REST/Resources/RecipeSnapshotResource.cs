using System.Collections.Generic;
using Hampcoders.Electrolink.API.Shared.Interfaces.REST.Resources;

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

