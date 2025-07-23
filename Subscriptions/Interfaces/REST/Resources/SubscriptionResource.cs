namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record SubscriptionResource(
    Guid Id,
    int UserId,
    Guid PlanId,
    string Status,
    DateTime ActivatedAt,
    bool IsPremium,
    bool IsCertified,
    bool CanUseBoost
);