namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents Customer Id in Stripe(formato: cus_xxxxx).
/// </summary>
public record PaymentGatewayCustomerId
{
    public string Value { get; init; }

    public PaymentGatewayCustomerId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stripe Customer ID cannot be empty", nameof(value));
        
        if (!value.StartsWith("cus_"))
            throw new ArgumentException("Invalid Stripe Customer ID format. Must start with 'cus_'", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}