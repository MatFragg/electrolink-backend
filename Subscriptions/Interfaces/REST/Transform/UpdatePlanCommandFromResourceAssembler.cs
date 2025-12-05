using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="UpdatePlanResource"/> to <see cref="UpdatePlanCommand"/>.
/// </summary>
public static class UpdatePlanCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a <see cref="UpdatePlanResource"/> to a <see cref="UpdatePlanCommand"/>.
    /// </summary>
    /// <param name="planId">The ID of the plan to update.</param>
    /// <param name="resource">The resource to convert.</param>
    /// <returns>The created command.</returns>
    public static UpdatePlanCommand ToCommand(Guid planId, UpdatePlanResource resource)
    {
        if (!Enum.TryParse(resource.MonetizationType, true, out EMonetizationType monetizationType))
        {
            throw new ArgumentException($"Invalid MonetizationType: {resource.MonetizationType}");
        }
        if (!Enum.TryParse(resource.TargetRole, true, out EUserRole targetRole))
        {
            throw new ArgumentException($"Invalid TargetRole: {resource.TargetRole}");
        }

        var benefits = resource.Benefits?.Select(b =>
            new Benefit(b.Type, b.LimitValue, b.FlagValue, b.Description)
        ).ToList() ?? new List<Benefit>();

        return new UpdatePlanCommand(
            planId,
            resource.Name,
            resource.Description,
            resource.Price,
            resource.Currency,
            monetizationType,
            targetRole,
            resource.IsDefault,
            benefits
        );
    }
}