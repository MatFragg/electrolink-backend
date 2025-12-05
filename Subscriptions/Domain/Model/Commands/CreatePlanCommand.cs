using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to create a new plan.
/// </summary>
/// <param name="Name">The name of the plan.</param>
/// <param name="Description">The description of the plan.</param>
/// <param name="Price">The price of the plan.</param>
/// <param name="Currency">The currency of the plan.</param>
/// <param name="MonetizationType">The monetization type.</param>
/// <param name="TargetRole">The target user role for the plan.</param>
/// <param name="IsDefault">Indicates if it's a default plan.</param>
/// <param name="Benefits">A list of benefits included in the plan.</param>
public record CreatePlanCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    EMonetizationType MonetizationType,
    EUserRole TargetRole,
    bool IsDefault,
    List<Benefit> Benefits,
    string StripePriceId
);