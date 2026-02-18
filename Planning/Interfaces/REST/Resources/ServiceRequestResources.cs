namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

/// <summary>
/// Resource for initiating a service request (Step 1)
/// </summary>
public record InitiateServiceRequestResource(
    Guid HomeownerId
);

/// <summary>
/// Resource for selecting property (Step 2)
/// </summary>
public record SelectPropertyResource(
    Guid PropertyId
);

/// <summary>
/// Resource for adding service details (Step 3-4)
/// </summary>
public record AddServiceDetailsResource(
    Guid SelectedRecipeId,
    Guid SelectedTechnicianId,
    ReceiptDataResource ReceiptData,
    bool IsPriority,
    RequestPreferencesResource Preferences
);

/// <summary>
/// Receipt data nested resource
/// </summary>
public record ReceiptDataResource(
    decimal ConsumptionKwh,
    decimal AmountPaid,
    string Currency,
    string BillingPeriod, // Format: YYYY-MM
    string ReceiptNumber
);

/// <summary>
/// Request preferences nested resource
/// </summary>
public record RequestPreferencesResource(
    List<DateTime> PreferredDates,
    string TimePreference, // "Morning", "Afternoon", "Anytime"
    string? ProblemDescription
);

/// <summary>
/// Resource returned for service request
/// </summary>
public record ServiceRequestResource(
    Guid RequestId,
    Guid HomeownerId,
    string Status,
    bool IsPriority,
    PropertySnapshotResource? PropertySnapshot,
    Guid? SelectedRecipeId,
    Guid? SelectedTechnicianId,
    ReceiptDataResource? ReceiptData,
    RequestPreferencesResource? Preferences,
    Guid? AssignedServiceId,
    string? CancellationReason
);

/// <summary>
/// Property snapshot nested resource
/// </summary>
public record PropertySnapshotResource(
    Guid PropertyId,
    string Address,
    double Latitude,
    double Longitude
);

/// <summary>
/// Simplified service request list resource
/// </summary>
public record ServiceRequestListResource(
    Guid RequestId,
    Guid HomeownerId,
    string Status,
    bool IsPriority,
    string? PropertyAddress,
    Guid? SelectedRecipeId,
    string? ServiceName
);

