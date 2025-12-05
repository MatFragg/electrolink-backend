using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="CreateSubscriptionResource"/> to <see cref="CreateSubscriptionCommand"/>.
/// </summary>
public static class CreateSubscriptionCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a <see cref="CreateSubscriptionResource"/> to a <see cref="CreateSubscriptionCommand"/>.
    /// </summary>
    /// <param name="resource">The resource to convert.</param>
    /// <returns>The created command.</returns>
    public static CreateSubscriptionCommand ToCommand(CreateSubscriptionResource resource)
    {
        if (!Enum.TryParse(resource.InitialStatus, true, out ESubscriptionStatus initialStatus))
        {
            throw new ArgumentException($"Invalid InitialStatus: {resource.InitialStatus}");
        }

        return new CreateSubscriptionCommand(
            resource.UserId,
            resource.PlanId,
            resource.StartDate,
            resource.EndDate,
            resource.StripeCustomerId,
            resource.StripeSubscriptionId,
            initialStatus,
            resource.TrialEndsAt
        );
    }
}