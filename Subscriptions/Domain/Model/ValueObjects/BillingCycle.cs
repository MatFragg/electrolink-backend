namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public enum EBillingCycle
{
    Monthly,
    Annual
}

public record BillingCycle
{
    public EBillingCycle Value { get; }

    private BillingCycle(EBillingCycle value)
    {
        Value = value;
    }

    public static BillingCycle Monthly => new(EBillingCycle.Monthly);
    public static BillingCycle Annual => new(EBillingCycle.Annual);

    public static BillingCycle From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("BillingCycle cannot be empty.");

        return value.Trim().ToUpperInvariant() switch
        {
            "MONTHLY" => Monthly,
            "ANNUAL" => Annual,
            _ => throw new ArgumentException($"Invalid BillingCycle: {value}")
        };
    }

    public override string ToString() => Value.ToString().ToUpperInvariant();
}
