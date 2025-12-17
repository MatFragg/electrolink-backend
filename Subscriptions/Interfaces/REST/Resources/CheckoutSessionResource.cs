namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Response resource with the Checkout URL.
/// </summary>
public record CheckoutSessionResource(
    string CheckoutUrl,
    string SessionId
);