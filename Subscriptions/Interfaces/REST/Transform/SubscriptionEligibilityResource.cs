using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionEligibilityAclAssembler
{
    /// <summary>
    /// Creates a basic <see cref="SubscriptionEligibilityResource"/> for a user without an active subscription.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A basic eligibility resource.</returns>
    public static SubscriptionEligibilityResource ToBasicResource(int userId)
    {
        return new SubscriptionEligibilityResource(
            UserId: userId,
            HasActiveSubscription: false,
            CurrentPlanName: "Free",
            IsPremiumUser: false,
            IsCertifiedTechnician: false,
            CanUseBoost: false,
            RemainingServiceRequests: null, // No limits for basic free tier
            PlanBenefits: new List<BenefitResource>()
        );
    }
    
    /// <summary>
    /// Creates a <see cref="SubscriptionEligibilityResource"/> from a <see cref="Subscription"/> and <see cref="Plan"/>.
    /// </summary>
    /// <param name="subscription">The user's subscription.</param>
    /// <param name="plan">The plan associated with the subscription.</param>
    /// <returns>An eligibility resource with detailed information.</returns>
    public static SubscriptionEligibilityResource ToResourceFromEntities(Subscription subscription, Plan plan)
    {
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);
        int? remainingServiceRequests = null;

        var maxServiceRequestsBenefit = plan.Benefits.FirstOrDefault(b => b.Type == "MaxServiceRequests");
        if (maxServiceRequestsBenefit != null && maxServiceRequestsBenefit.LimitValue.HasValue)
        {
            remainingServiceRequests = maxServiceRequestsBenefit.LimitValue.Value - subscription.CurrentUsage;
        }

        var benefitResources = plan.Benefits.Select(b => new BenefitResource(
            b.Type, b.LimitValue, b.FlagValue, b.Description
        )).ToList();

        return new SubscriptionEligibilityResource(
            UserId: subscription.UserId.Value,
            HasActiveSubscription: subscription.Status == ESubscriptionStatus.Active || subscription.Status == ESubscriptionStatus.Trial,
            CurrentPlanName: plan.Name,
            IsPremiumUser: isPremium,
            IsCertifiedTechnician: isCertified,
            CanUseBoost: canUseBoost,
            RemainingServiceRequests: remainingServiceRequests,
            PlanBenefits: benefitResources
        );
    }
}