using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

/// <summary>
/// Integration Event: A subscription was canceled.
/// Used by Dashboard, Profiles (to revert permissions).
/// </summary>
public record SubscriptionCancelledIntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = nameof(SubscriptionCancelledIntegrationEvent);
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOn { get; init; }

    public Guid SubscriptionId { get; init; }
    public int UserId { get; init; }
    public DateTime CancellationEffectiveDate { get; init; }
    public string Reason { get; init; }

    public SubscriptionCancelledIntegrationEvent(
        Guid subscriptionId,
        int userId,
        DateTime cancellationEffectiveDate,
        string reason,
        DateTime occurredOn)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        CancellationEffectiveDate = cancellationEffectiveDate;
        Reason = reason ?? "User initiated";
        OccurredOn = occurredOn;
    }
}