using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class InitiateCheckoutCommandFromResourceAssembler
{
    public static InitiateCheckoutCommand ToCommand(string userId, InitiateCheckoutResource resource)
        => new(
            UserId: userId,
            BillingCycle: resource.BillingCycle,
            SuccessUrl: resource.SuccessUrl,
            CancelUrl: resource.CancelUrl);
}

