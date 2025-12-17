namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Resource for internal plan changes (non-Stripe).
/// </summary>
public record ChangeSubscriptionPlanInternalResource(
    Guid NewPlanId,
    DateTime NewEndDate,
    string StripeSubscriptionId
);