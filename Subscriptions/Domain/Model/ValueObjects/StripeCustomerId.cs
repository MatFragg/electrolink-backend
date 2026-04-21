namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing Stripe's unique customer identifier.
/// </summary>
public record StripeCustomerId
{
    public string Value { get; }

    private StripeCustomerId(string value) => Value = value;

    /// <summary>
    /// Factory method to create StripeCustomerId from string value.
    /// </summary>
    public static StripeCustomerId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StripeCustomerId cannot be empty.");
        return new StripeCustomerId(value);
    }

    public override string ToString() => Value;
}

