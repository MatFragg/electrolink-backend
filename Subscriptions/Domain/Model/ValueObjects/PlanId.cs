namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Not used in tactical domain.
/// </summary>
public record PlanId
{
    public Guid Value { get; }

    public PlanId(Guid value)
    {
        Value = value;
    }

    public static PlanId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}

