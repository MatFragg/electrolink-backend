namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record CheckoutSessionMetadata(
    PaymentGatewayCustomerId? CustomerId,
    SubscriptionId? SubscriptionId,
    Dictionary<string, string>? AdditionalData = null
);