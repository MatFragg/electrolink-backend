namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record MySubscriptionResource(
    string SubscriptionId,
    string PlanType,
    string Status,
    string? BillingCycle,
    string? PeriodEnd,
    bool CancelAtPeriodEnd,
    int? MonthlyRequestsUsed,
    int? MonthlyRequestsLimit,
    string? GracePeriodEndsAt);

