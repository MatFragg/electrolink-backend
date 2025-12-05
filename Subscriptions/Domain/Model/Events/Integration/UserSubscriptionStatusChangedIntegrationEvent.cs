using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

public record UserSubscriptionStatusChangedIntegrationEvent(
    int UserId, // The user ID (int) from IAM/Profiles
    string NewSubscriptionStatus, // String representation of ESubscriptionStatus
    bool IsPremium, // Derived from the new plan type (e.g., if plan is not Free)
    bool IsCertified, // Derived from plan benefits or subscription state
    bool CanUseBoost, // Derived from plan benefits or subscription state
    DateTime OccurredOn) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
