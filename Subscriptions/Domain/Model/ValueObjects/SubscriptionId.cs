namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing a subscription identifier with "subs-" prefix.
/// Format: "subs-{Guid}"
/// </summary>
public record SubscriptionId
{
    public string Value { get; }

    private SubscriptionId(string value) => Value = value;

    /// <summary>
    /// Factory method to generate a new SubscriptionId with unique Guid.
    /// </summary>
    public static SubscriptionId NewSubscriptionId() =>
        new($"subs-{Guid.NewGuid()}");

    /// <summary>
    /// Factory method to create SubscriptionId from existing string value.
    /// Validates that value starts with "subs-" prefix.
    /// </summary>
    public static SubscriptionId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("subs-"))
            throw new ArgumentException("Invalid SubscriptionId format. Must start with 'subs-'.");
        return new SubscriptionId(value);
    }

    public override string ToString() => Value;
}