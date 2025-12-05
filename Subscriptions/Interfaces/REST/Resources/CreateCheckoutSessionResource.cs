namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record CreateCheckoutSessionResource(
    string PriceId, 
    string SuccessUrl, 
    string CancelUrl
);