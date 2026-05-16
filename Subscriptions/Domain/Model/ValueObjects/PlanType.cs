namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public enum EPlanType
{
    Basic,
    Premium,
    Enterprise
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
    public static PlanType Enterprise => new(EPlanType.Enterprise);

    public bool IsBasic => Value == EPlanType.Basic;
    public bool IsPremium => Value == EPlanType.Premium;
    public bool IsEnterprise => Value == EPlanType.Enterprise;

    public static PlanType From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PlanType cannot be empty.");

        return value.Trim().ToUpperInvariant() switch
        {
            "BASIC" => Basic,
            "PREMIUM" => Premium,
            "ENTERPRISE" => Enterprise,
            _ => throw new ArgumentException($"Invalid PlanType: {value}")
        };
    }

    public override string ToString() => Value.ToString().ToUpperInvariant();
}
