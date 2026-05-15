using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class MySubscriptionResourceFromEntityAssembler
{
    public static MySubscriptionResource ToResource(Subscription subscription)
        => new(
            SubscriptionId: subscription.SubscriptionId.Value,
            PlanType: subscription.PlanType.ToString(),
            Status: subscription.Status.ToString(),
            BillingCycle: subscription.BillingCycle?.ToString(),
            PeriodEnd: subscription.BillingPeriod?.PeriodEnd.ToString("O"),
            CancelAtPeriodEnd: subscription.CancelAtPeriodEnd,
            MonthlyRequestsUsed: subscription.UsageCounters?.MonthlyRequestsUsed,
            MonthlyRequestsLimit: subscription.UsageCounters?.MonthlyRequestsLimit,
            GracePeriodEndsAt: subscription.GracePeriodEndsAt?.ToString("O"));
}

