using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to create a Checkout session in Stripe.
/// </summary>
public record CreateCheckoutSessionCommand(
    string UserId,
    PlanId PlanId,
    string SuccessUrl,
    string CancelUrl,
    int? TrialPeriodDays = null
);