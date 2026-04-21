namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing Stripe's unique checkout session identifier.
/// </summary>
public record StripeCheckoutSessionId
{
    public string Value { get; }

    private StripeCheckoutSessionId(string value) => Value = value;

    /// <summary>
    /// Factory method to create StripeCheckoutSessionId from string value.
    /// </summary>
    public static StripeCheckoutSessionId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StripeCheckoutSessionId cannot be empty.");
        return new StripeCheckoutSessionId(value);
    }

    public override string ToString() => Value;
}

