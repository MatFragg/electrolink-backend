using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class PlanResourceFromEntityAssembler
{
    public static PlanResource ToResource(Plan plan)
    {
        return new PlanResource(
            plan.Id.Value,
            plan.Name,
            plan.Description,
            plan.Price,
            plan.Currency,
            plan.MonetizationType.ToString(),
            plan.IsDefault
        );
    }
}