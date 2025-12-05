namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for creating a new plan.
/// </summary>
/// <param name="Name">The name of the plan.</param>
/// <param name="Description">The description of the plan.</param>
/// <param name="Price">The price of the plan.</param>
/// <param name="Currency">The currency of the plan.</param>
/// <param name="MonetizationType">The monetization type (e.g., "Free", "Monthly").</param>
/// <param name="IsDefault">Indicates if it's a default plan.</param>
/// <param name="TargetRole">The target user role for the plan (e.g., "Homeowner", "Technician").</param>
/// <param name="Benefits">A list of benefits included in the plan.</param>
public record CreatePlanResource(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault,
    string TargetRole,
    List<BenefitResource> Benefits, 
    string? StripePriceId
);