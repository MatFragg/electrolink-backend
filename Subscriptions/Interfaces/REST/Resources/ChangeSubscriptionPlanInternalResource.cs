namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for internal plan changes (non-Stripe).
/// </summary>
public record ChangeSubscriptionPlanInternalResource(
    Guid NewPlanId,
    DateTime NewEndDate,
    string StripeSubscriptionId
);