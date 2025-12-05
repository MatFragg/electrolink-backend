using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;

/// <summary>
/// Entity that represents a processed Stripe webhook event.
/// Purpose: Ensure idempotency (each event is processed only once).
/// Note: This is a simple Entity, not an Aggregate, because it has no complex
/// business invariants and does not coordinate other entities.
/// </summary>
public class WebhookEvent
{
    /// <summary>
    /// Event id of the Stripe event (evt_xxxxx).
    /// </summary>
    public WebhookEventId Id { get; private set; }

    /// <summary>
    /// Type of event (e.g., customer.subscription.updated).
    /// </summary>
    public string EventType { get; private set; }

    /// <summary>
    /// Complete payload of the event in JSON.
    /// </summary>
    public string RawPayload { get; private set; }

    /// <summary>
    /// Date when Stripe created the event.
    /// </summary>
    public DateTime StripeCreatedAt { get; private set; }

    /// <summary>
    /// Date when we processed the event.
    /// </summary>
    public DateTime? ProcessedAt { get; private set; }

    /// <summary>
    /// Indicates whether the event was processed successfully.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Error message if processing failed.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Number of processing attempts.
    /// </summary>
    public int ProcessingAttempts { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private WebhookEvent() { }

    /// <summary>
    /// Constructor for new webhook event.
    /// </summary>
    public WebhookEvent(
        WebhookEventId id,
        string eventType,
        string rawPayload,
        DateTime stripeCreatedAt)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
        RawPayload = rawPayload ?? throw new ArgumentNullException(nameof(rawPayload));
        StripeCreatedAt = stripeCreatedAt;
        IsProcessed = false;
        ProcessingAttempts = 0;
    }

    /// <summary>
    /// Marks the event as processed successfully.
    /// </summary>
    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedAt = DateTime.UtcNow;
        ErrorMessage = null;
    }

    /// <summary>
    /// Records a failed processing attempt..
    /// </summary>
    public void RecordFailedAttempt(string errorMessage)
    {
        ProcessingAttempts++;
        ErrorMessage = errorMessage;
        ProcessedAt = DateTime.UtcNow;

        // Marks as processed after 5 attempts to avoid infinite loops
        if (ProcessingAttempts >= 5)
        {
            IsProcessed = true;
        }
    }

    /// <summary>
    /// Verifies if the event has already been processed or exceeded attempts.s.
    /// </summary>
    public bool ShouldSkipProcessing() => IsProcessed || ProcessingAttempts >= 5;
}