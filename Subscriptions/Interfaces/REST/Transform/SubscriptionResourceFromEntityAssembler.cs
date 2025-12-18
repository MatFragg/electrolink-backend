using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="Subscription"/> entity and <see cref="Plan"/> entity to <see cref="SubscriptionResource"/>.
/// </summary>
public static class SubscriptionResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a <see cref="Subscription"/> aggregate and its associated <see cref="Plan"/> to a <see cref="SubscriptionResource"/>.
    /// </summary>
    /// <param name="subscription">The subscription aggregate to convert.</param>
    /// <param name="plan">The associated plan aggregate.</param>
    /// <returns>The created subscription resource.</returns>
    public static SubscriptionResource ToResourceFromEntity(Subscription subscription, Plan plan)
    {
        // Determine derived properties based on plan benefits and subscription state
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        return new SubscriptionResource(
            subscription.Id.Value,
            subscription.UserId.Value,
            subscription.PlanId.Value,
            subscription.Status.ToString(),
            subscription.StartDate,
            subscription.EndDate,
            subscription.CurrentUsage,
            isPremium,
            isCertified,
            canUseBoost
        );
    }
}