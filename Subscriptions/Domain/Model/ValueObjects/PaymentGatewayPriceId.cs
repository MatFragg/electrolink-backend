namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents Price Id in Stripe (formato: price_xxxxx).
/// </summary>
public record PaymentGatewayPriceId
{
    public string Value { get; init; }

    public PaymentGatewayPriceId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stripe Price ID cannot be empty", nameof(value));
        
        if (!value.StartsWith("price_"))
            throw new ArgumentException("Invalid Stripe Price ID format. Must start with 'price_'", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}