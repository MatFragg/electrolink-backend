namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record SubscriptionStatusAlertResource(
    string Status,
    string PlanType,
    string GracePeriodEndsAt,
    string Message,
    string? CustomerPortalUrl);

