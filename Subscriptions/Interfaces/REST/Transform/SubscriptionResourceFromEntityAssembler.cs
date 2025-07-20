using Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResource(Domain.Model.Aggregates.Subscription subscription)
    {
        return new SubscriptionResource(
            subscription.Id.Value,
            subscription.UserId.Value,
            subscription.PlanId.Value,
            subscription.Status.ToString(),
            subscription.ActivatedAt,
            subscription.PremiumAccess?.IsActive(DateTime.UtcNow) ?? false,
            subscription.Certification?.IsVerified  ?? false,
            subscription.Boost?.CanActivate(DateTime.UtcNow) ?? false
        );
    }
}