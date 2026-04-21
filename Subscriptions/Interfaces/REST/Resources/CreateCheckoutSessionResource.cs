namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource to create a Stripe Checkout session.
/// </summary>
public record CreateCheckoutSessionResource(
    string PriceId,
    string SuccessUrl,
    string CancelUrl,
    int? TrialPeriodDays = null
);