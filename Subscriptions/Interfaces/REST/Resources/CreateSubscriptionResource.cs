namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for creating a new subscription.
/// </summary>
/// <param name="UserId">The ID of the user (int).</param>
/// <param name="PlanId">The ID of the plan (Guid).</param>
/// <param name="StartDate">The start date of the subscription.</param>
/// <param name="EndDate">The end date of the subscription.</param>
/// <param name="StripeCustomerId">Stripe's customer ID.</param>
/// <param name="StripeSubscriptionId">Stripe's subscription ID.</param>
/// <param name="InitialStatus">The initial status of the subscription (e.g., "Active", "Trial").</param>
/// <param name="TrialEndsAt">Optional trial end date.</param>
public record CreateSubscriptionResource(
    int UserId,
    Guid PlanId,
    DateTime StartDate,
    DateTime EndDate,
    string StripeCustomerId,
    string StripeSubscriptionId,
    string InitialStatus,
    DateTime? TrialEndsAt = null
);