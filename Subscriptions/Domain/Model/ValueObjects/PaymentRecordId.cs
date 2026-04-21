namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record PaymentRecordId
{
    public string Value { get; }

    private PaymentRecordId(string value)
    {
        Value = value;
    }

    public static PaymentRecordId NewPaymentRecordId() => new($"pay-{Guid.NewGuid()}");

    public static PaymentRecordId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("pay-"))
            throw new ArgumentException("Invalid PaymentRecordId format.");

        return new PaymentRecordId(value);
    }

    public override string ToString() => Value;
}

