namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Not used in tactical design.
/// </summary>
public record PaymentGatewayPriceId
{
    public string Value { get; }

    public PaymentGatewayPriceId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PaymentGatewayPriceId cannot be empty.");
        Value = value;
    }

    public static PaymentGatewayPriceId From(string value) => new(value);
    public override string ToString() => Value;
}

