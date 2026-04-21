namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Initiate Stripe checkout for PREMIUM upgrade from BASIC.
/// </summary>
public record InitiateCheckoutCommand(
    string UserId,
    string BillingCycle,
    string SuccessUrl,
    string CancelUrl);
