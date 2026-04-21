namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Use StripeSubscriptionId instead.
/// Value Object representing Stripe's unique subscription identifier.
/// </summary>
public record PaymentGatewaySubscriptionId
{
    public string Value { get; }

    public PaymentGatewaySubscriptionId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PaymentGatewaySubscriptionId cannot be empty.");
        Value = value;
    }

    public static PaymentGatewaySubscriptionId From(string value) => new(value);
    public override string ToString() => Value;

    // Implicit conversion to StripeSubscriptionId for migration
    public static implicit operator StripeSubscriptionId?(PaymentGatewaySubscriptionId? id) => 
        id == null ? null : StripeSubscriptionId.From(id.Value);
    public static implicit operator PaymentGatewaySubscriptionId?(StripeSubscriptionId? id) => 
        id == null ? null : new PaymentGatewaySubscriptionId(id.Value);
}

