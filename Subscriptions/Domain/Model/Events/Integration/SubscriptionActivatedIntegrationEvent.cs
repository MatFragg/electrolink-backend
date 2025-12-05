using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

/// <summary>
/// Integration Event: A subscription was created successfully.
/// Used by Dashboard, Profiles, IAM.
/// </summary>
public record SubscriptionActivatedIntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = nameof(SubscriptionActivatedIntegrationEvent);
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOn { get; init; }

    public Guid SubscriptionId { get; init; }
    public int UserId { get; init; }
    public Guid PlanId { get; init; }
    public string PlanName { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool IsTrial { get; init; }
    
    public List<string> Benefits { get; init; } // Lista de benefit types

    public SubscriptionActivatedIntegrationEvent(
        Guid subscriptionId,
        int userId,
        Guid planId,
        string planName,
        DateTime startDate,
        DateTime endDate,
        bool isTrial,
        List<string> benefits,
        DateTime occurredOn)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        PlanId = planId;
        PlanName = planName;
        StartDate = startDate;
        EndDate = endDate;
        IsTrial = isTrial;
        Benefits = benefits ?? new List<string>();
        OccurredOn = occurredOn;
    }
}