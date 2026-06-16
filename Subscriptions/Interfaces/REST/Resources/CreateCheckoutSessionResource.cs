namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Resource to create a Stripe Checkout session.
/// </summary>
public record CreateCheckoutSessionResource(
    [property: Required] string PriceId,
    [property: Required, Url] string SuccessUrl,
    [property: Required, Url] string CancelUrl,
    int? TrialPeriodDays = null
);