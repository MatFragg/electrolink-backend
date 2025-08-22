namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record SubscriptionEligibilityResource(
    int UserId, 
    string PlanName,
    bool IsPremium,
    int? UsageLimit,
    int? CurrentUsage,
    bool CanCreatePriorityRequest
);