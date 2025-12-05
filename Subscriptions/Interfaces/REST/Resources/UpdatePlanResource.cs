namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for updating an existing plan.
/// </summary>
/// <param name="Name">The new name of the plan.</param>
/// <param name="Description">The new description of the plan.</param>
/// <param name="Price">The new price of the plan.</param>
/// <param name="Currency">The new currency of the plan.</param>
/// <param name="MonetizationType">The new monetization type.</param>
/// <param name="IsDefault">The new default status.</param>
/// <param name="TargetRole">The new target user role for the plan.</param>
/// <param name="Benefits">The updated list of benefits.</param>
public record UpdatePlanResource(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault,
    string TargetRole,
    List<BenefitResource> Benefits
);