namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for representing a plan in API responses.
/// </summary>
/// <param name="Name">The name of the plan.</param>
/// <param name="Description">A detailed description of the plan.</param>
/// <param name="Price">The price of the plan.</param>
/// <param name="Currency">The currency of the plan's price.</param>
/// <param name="MonetizationType">The monetization type (e.g., "Monthly").</param>
/// <param name="IsDefault">Indicates if this is a default plan.</param>
/// <param name="TargetRole">The target user role for this plan.</param>
/// <param name="Benefits">A collection of benefits included in this plan.</param>
/// <param name="StripePriceId">The price Id </param>
public record PlanResource(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault,
    string TargetRole,
    string? StripePriceId,
    List<BenefitResource> Benefits
);