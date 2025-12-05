namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Representa the unique ID of a Stripe webhook event.
/// Used to garantice idempotency.
/// </summary>
public record WebhookEventId
{
    public string Value { get; init; }

    public WebhookEventId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Webhook Event ID cannot be empty", nameof(value));
        
        if (!value.StartsWith("evt_"))
            throw new ArgumentException("Invalid Stripe Event ID format. Must start with 'evt_'", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}

