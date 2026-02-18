namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

/// <summary>
/// Resource returned for service assignment
/// </summary>
public record ServiceAssignmentResource(
    Guid ServiceId,
    Guid RequestId,
    Guid TechnicianId,
    Guid HomeownerId,
    Guid PropertyId,
    RecipeSnapshotResource RecipeSnapshot,
    DateTime ScheduledStartDateTime,
    DateTime ScheduledEndDateTime,
    string Status,
    bool IsPriority
);

/// <summary>
/// Recipe snapshot nested resource
/// </summary>
public record RecipeSnapshotResource(
    Guid RecipeId,
    string ServiceName,
    string ServiceCategory,
    List<ComponentRequirementResource> ComponentRequirements,
    decimal TotalPrice,
    string Currency,
    int EstimatedDurationMinutes,
    int WarrantyMonths
);

/// <summary>
/// Simplified assignment list resource
/// </summary>
public record ServiceAssignmentListResource(
    Guid ServiceId,
    Guid RequestId,
    string ServiceName,
    Guid TechnicianId,
    Guid HomeownerId,
    DateTime ScheduledStartDateTime,
    DateTime ScheduledEndDateTime,
    string Status,
    bool IsPriority
);

