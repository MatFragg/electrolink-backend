namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents el PaymentIntent Id in Stripe (formato: pi_xxxxx).
/// </summary>
public record StripePaymentIntentId
{
    public string Value { get; init; }

    public StripePaymentIntentId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stripe PaymentIntent ID cannot be empty", nameof(value));
        
        if (!value.StartsWith("pi_") && !value.StartsWith("in_")) // in_ para invoices
            throw new ArgumentException("Invalid Stripe PaymentIntent ID format", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}