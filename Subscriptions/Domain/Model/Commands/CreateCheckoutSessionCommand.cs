namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to create a Checkout session in Stripe.
/// </summary>
public record CreateCheckoutSessionCommand(
    int UserId,
    Guid PlanId,
    string SuccessUrl,
    string CancelUrl,
    int? TrialPeriodDays = null
);