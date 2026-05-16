namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record StripePaymentIntentId
{
    public string Value { get; }

    private StripePaymentIntentId(string value) => Value = value;

    public static StripePaymentIntentId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stripe PaymentIntent ID cannot be empty.");
        return new StripePaymentIntentId(value.Trim());
    }

    public static StripePaymentIntentId NewPaymentIntentId() => From($"pi_{Guid.NewGuid():N}");
}
