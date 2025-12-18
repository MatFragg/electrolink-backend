using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="Plan"/> entity to <see cref="PlanResource"/>.
/// </summary>
public static class PlanResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a <see cref="Plan"/> aggregate to a <see cref="PlanResource"/>.
    /// </summary>
    /// <param name="plan">The plan aggregate to convert.</param>
    /// <returns>The created plan resource.</returns>
    public static PlanResource ToResourceFromEntity(Plan plan)
    {
        var benefitResources = plan.Benefits?.Select(b =>
            new BenefitResource(b.Type, b.LimitValue, b.FlagValue, b.Description)
        ).ToList() ?? new List<BenefitResource>();

        return new PlanResource(
            plan.Name,
            plan.Description,
            plan.Price,
            plan.Currency,
            plan.MonetizationType.ToString(),
            plan.IsDefault,
            plan.TargetRole.ToString(),
            plan.GatewayPriceId?.Value, 
            benefitResources
        );
    }
}