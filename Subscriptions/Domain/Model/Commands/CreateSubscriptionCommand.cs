using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to create a new subscription.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
/// <param name="PlanId">The ID of the plan.</param>
/// <param name="StartDate">The start date of the subscription.</param>
/// <param name="EndDate">The end date of the subscription.</param>
/// <param name="StripeCustomerId">Stripe's customer ID.</param>
/// <param name="StripeSubscriptionId">Stripe's subscription ID.</param>
/// <param name="InitialStatus">The initial status of the subscription.</param>
/// <param name="TrialEndsAt">Optional trial end date.</param>
public record CreateSubscriptionCommand(
    int UserId,
    Guid PlanId,
    DateTime StartDate,
    DateTime EndDate,
    string StripeCustomerId,
    string StripeSubscriptionId,
    ESubscriptionStatus InitialStatus,
    DateTime? TrialEndsAt = null
);