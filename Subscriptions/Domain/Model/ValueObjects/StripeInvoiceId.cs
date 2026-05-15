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
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StripeInvoiceId cannot be empty.");

        return new StripeInvoiceId(value);
    }

    public override string ToString() => Value;
}
