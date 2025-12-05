using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

/// <summary>
/// Integration Event: User changed his plan (upgrade/downgrade).
/// Consumed by: Dashboard, Profiles (update permission).
/// </summary>
public record PlanChangedIntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = nameof(PlanChangedIntegrationEvent);
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOn { get; init; }

    public Guid SubscriptionId { get; init; }
    public int UserId { get; init; }
    public Guid OldPlanId { get; init; }
    public string OldPlanName { get; init; }
    public Guid NewPlanId { get; init; }
    public string NewPlanName { get; init; }
    public DateTime EffectiveDate { get; init; }
    public List<string> NewBenefits { get; init; }

    public PlanChangedIntegrationEvent(
        Guid subscriptionId,
        int userId,
        Guid oldPlanId,
        string oldPlanName,
        Guid newPlanId,
        string newPlanName,
        DateTime effectiveDate,
        List<string> newBenefits,
        DateTime occurredOn)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        OldPlanId = oldPlanId;
        OldPlanName = oldPlanName;
        NewPlanId = newPlanId;
        NewPlanName = newPlanName;
        EffectiveDate = effectiveDate;
        NewBenefits = newBenefits ?? new List<string>();
        OccurredOn = occurredOn;
    }
}