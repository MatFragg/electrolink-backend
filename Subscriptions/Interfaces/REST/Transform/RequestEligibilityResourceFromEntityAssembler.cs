using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class RequestEligibilityResourceFromEntityAssembler
{
    public static RequestEligibilityResource ToResource(RequestEligibilityResult result)
        => new(
            CanRequest: result.CanRequest,
            IsPriorityAllowed: result.IsPriorityAllowed,
            RemainingRequests: result.RemainingRequests,
            PlanType: result.PlanType,
            UpgradeRequired: result.UpgradeRequired,
            UpgradePromptMessage: result.UpgradeRequired
                ? "Has usado tus 2 solicitudes gratuitas este mes. Actualiza a Premium para solicitar sin límites."
                : null);
}
