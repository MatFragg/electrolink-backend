namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record InitiateCheckoutResource(
    string BillingCycle,
    string SuccessUrl,
    string CancelUrl);

