using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="CreatePlanResource"/> to <see cref="CreatePlanCommand"/>.
/// </summary>
public static class CreatePlanCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a <see cref="CreatePlanResource"/> to a <see cref="CreatePlanCommand"/>.
    /// </summary>
    /// <param name="resource">The resource to convert.</param>
    /// <returns>The created command.</returns>
    public static CreatePlanCommand ToCommand(CreatePlanResource resource)
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

        return new CreatePlanCommand(
            resource.Name,
            resource.Description,
            resource.Price,
            resource.Currency,
            monetizationType,
            targetRole,
            resource.IsDefault,
            benefits,
            resource.StripePriceId
        );
    }
}