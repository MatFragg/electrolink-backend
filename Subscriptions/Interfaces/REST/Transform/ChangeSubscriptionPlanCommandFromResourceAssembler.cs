using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;


/// <summary>
/// Assembler for converting <see cref="ChangeSubscriptionPlanResource"/> to <see cref="ChangeSubscriptionPlanCommand"/>.
/// </summary>
public static class ChangeSubscriptionPlanCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a <see cref="ChangeSubscriptionPlanResource"/> to a <see cref="ChangeSubscriptionPlanCommand"/>.
    /// </summary>
    /// <param name="subscriptionId">The ID of the subscription to change.</param>
    /// <param name="resource">The resource containing the new plan details.</param>
    /// <returns>The created command.</returns>
    public static ChangeSubscriptionPlanCommand ToCommand(
        Guid subscriptionId, 
        ChangeSubscriptionPlanInternalResource resource)
    {
        return new ChangeSubscriptionPlanCommand(
            new SubscriptionId(subscriptionId),
            resource.NewPlanId,
            resource.NewEndDate,
            new PaymentGatewaySubscriptionId(resource.StripeSubscriptionId)
        );
    }
}