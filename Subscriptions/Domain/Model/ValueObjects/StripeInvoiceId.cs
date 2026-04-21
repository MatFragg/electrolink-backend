namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record StripeInvoiceId
{
    public string Value { get; }

    private StripeInvoiceId(string value)
    {
        Value = value;
    }

    public static StripeInvoiceId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("in_"))
            throw new ArgumentException("Invalid StripeInvoiceId format.");

        return new StripeInvoiceId(value);
    }

    public override string ToString() => Value;
}

