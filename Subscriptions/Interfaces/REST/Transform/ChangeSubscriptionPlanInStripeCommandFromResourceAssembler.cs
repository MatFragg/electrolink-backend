using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting Stripe plan change resources to commands.
/// Used for plan changes that interact with Stripe API.
/// </summary>
public static class ChangeSubscriptionPlanInStripeCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a Stripe resource to a command for Stripe-based plan changes.
    /// </summary>
    public static ChangeSubscriptionPlanInGatewayCommand ToCommand(
        SubscriptionId subscriptionId, 
        ChangeSubscriptionPlanResource resource)
    {
        return new ChangeSubscriptionPlanInGatewayCommand(
            subscriptionId,
            new PlanId(resource.NewPlanId),
            resource.ProrationBehavior
        );
    }
}