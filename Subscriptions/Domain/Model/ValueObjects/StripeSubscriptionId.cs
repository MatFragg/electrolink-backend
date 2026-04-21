namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing Stripe's unique subscription identifier.
/// Nullable since BASIC tier has no Stripe subscription.
/// </summary>
public record StripeSubscriptionId
{
    public string Value { get; }

    private StripeSubscriptionId(string value) => Value = value;

    /// <summary>
    /// Factory method to create StripeSubscriptionId from string value.
    /// </summary>
    public static StripeSubscriptionId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StripeSubscriptionId cannot be empty.");
        return new StripeSubscriptionId(value);
    }

    public override string ToString() => Value;
}

