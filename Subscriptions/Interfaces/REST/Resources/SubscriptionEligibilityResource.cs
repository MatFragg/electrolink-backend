namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for displaying a user's subscription eligibility and current benefits.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
/// <param name="HasActiveSubscription">True if the user has an active subscription.</param>
/// <param name="CurrentPlanName">The name of the current plan.</param>
/// <param name="IsPremiumUser">True if the user has premium access.</param>
/// <param name="IsCertifiedTechnician">True if the user is a certified technician.</param>
/// <param name="CanUseBoost">True if the user can use the boost feature.</param>
/// <param name="RemainingServiceRequests">Number of remaining service requests (if applicable).</param>
/// <param name="PlanBenefits">List of benefits provided by the current plan.</param>
public record SubscriptionEligibilityResource(
    int UserId,
    bool HasActiveSubscription,
    string CurrentPlanName,
    bool IsPremiumUser,
    bool IsCertifiedTechnician,
    bool CanUseBoost,
    int? RemainingServiceRequests,
    List<BenefitResource> PlanBenefits
);