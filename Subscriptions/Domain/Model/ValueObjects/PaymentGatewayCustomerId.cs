namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Use StripeCustomerId instead.
/// Value Object representing Stripe's unique customer identifier.
/// </summary>
public record PaymentGatewayCustomerId
{
    public string Value { get; }

    public PaymentGatewayCustomerId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PaymentGatewayCustomerId cannot be empty.");
        Value = value;
    }

    public static PaymentGatewayCustomerId From(string value) => new(value);
    public override string ToString() => Value;

    // Implicit conversion to StripeCustomerId for migration
    public static implicit operator StripeCustomerId(PaymentGatewayCustomerId id) => StripeCustomerId.From(id.Value);
    public static implicit operator PaymentGatewayCustomerId(StripeCustomerId id) => new(id.Value);
}

