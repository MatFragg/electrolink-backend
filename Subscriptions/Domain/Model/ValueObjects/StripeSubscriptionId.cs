namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents Subscription Id in Stripe (formato: sub_xxxxx).
/// </summary>
public record StripeSubscriptionId
{
    public string Value { get; init; }

    public StripeSubscriptionId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stripe Subscription ID cannot be empty", nameof(value));
        
        if (!value.StartsWith("sub_"))
            throw new ArgumentException("Invalid Stripe Subscription ID format. Must start with 'sub_'", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}