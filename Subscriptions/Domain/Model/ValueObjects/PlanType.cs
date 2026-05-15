namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public enum EPlanType
{
    Basic,
    Premium
}

public record PlanType
{
    public EPlanType Value { get; }

    private PlanType(EPlanType value)
    {
        Value = value;
    }

    public static PlanType Basic => new(EPlanType.Basic);
    public static PlanType Premium => new(EPlanType.Premium);

    public bool IsBasic => Value == EPlanType.Basic;
    public bool IsPremium => Value == EPlanType.Premium;

    public static PlanType From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PlanType cannot be empty.");

        return value.Trim().ToUpperInvariant() switch
        {
            "BASIC" => Basic,
            "PREMIUM" => Premium,
            _ => throw new ArgumentException($"Invalid PlanType: {value}")
        };
    }

    public override string ToString() => Value.ToString().ToUpperInvariant();
}
