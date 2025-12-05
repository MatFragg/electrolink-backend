using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to update an existing plan.
/// </summary>
/// <param name="PlanId">The ID of the plan to update.</param>
/// <param name="Name">The new name of the plan.</param>
/// <param name="Description">The new description of the plan.</param>
/// <param name="Price">The new price of the plan.</param>
/// <param name="Currency">The new currency of the plan.</param>
/// <param name="MonetizationType">The new monetization type.</param>
/// <param name="TargetRole">The new target user role for the plan.</param>
/// <param name="IsDefault">The new default status.</param>
/// <param name="Benefits">The updated list of benefits.</param>
public record UpdatePlanCommand(
    Guid PlanId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    EMonetizationType MonetizationType,
    EUserRole TargetRole,
    bool IsDefault,
    List<Benefit> Benefits
);