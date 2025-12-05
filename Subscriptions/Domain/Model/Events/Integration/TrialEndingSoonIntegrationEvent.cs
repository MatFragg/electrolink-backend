using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

/// <summary>
/// Integration Event: Trial period is about to end (3 days before).
/// Consumed By: IAM (send email).
/// </summary>
public record TrialEndingSoonIntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = nameof(TrialEndingSoonIntegrationEvent);
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOn { get; init; }

    public Guid SubscriptionId { get; init; }
    public int UserId { get; init; }
    public DateTime TrialEndDate { get; init; }
    public int DaysRemaining { get; init; }

    public TrialEndingSoonIntegrationEvent(
        Guid subscriptionId,
        int userId,
        DateTime trialEndDate,
        int daysRemaining,
        DateTime occurredOn)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        TrialEndDate = trialEndDate;
        DaysRemaining = daysRemaining;
        OccurredOn = occurredOn;
    }
}