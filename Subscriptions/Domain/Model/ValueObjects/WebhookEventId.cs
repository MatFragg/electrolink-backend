namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Not used in tactical domain.
/// </summary>
public record WebhookEventId
{
    public string Value { get; }

    public WebhookEventId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("WebhookEventId cannot be empty.");
        Value = value;
    }

    public static WebhookEventId NewWebhookEventId() => new($"whe-{Guid.NewGuid()}");
    public static WebhookEventId From(string value) => new(value);
    public override string ToString() => Value;
}
