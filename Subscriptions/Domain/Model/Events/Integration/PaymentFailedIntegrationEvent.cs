using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

/// <summary>
/// Integration Event: A payment failed.
/// Consumed by: IAM (enviar email), Dashboard (show alert).
/// </summary>
public record PaymentFailedIntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = nameof(PaymentFailedIntegrationEvent);
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOn { get; init; }

    public Guid SubscriptionId { get; init; }
    public int UserId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public string FailureReason { get; init; }
    public DateTime NextRetryDate { get; init; }

    public PaymentFailedIntegrationEvent(
        Guid subscriptionId,
        int userId,
        decimal amount,
        string currency,
        string failureReason,
        DateTime nextRetryDate,
        DateTime occurredOn)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        Amount = amount;
        Currency = currency;
        FailureReason = failureReason;
        NextRetryDate = nextRetryDate;
        OccurredOn = occurredOn;
    }
}