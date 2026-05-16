namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record InitiateCheckoutResult(string CheckoutUrl, string SessionId);
