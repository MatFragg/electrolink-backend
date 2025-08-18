using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;

public record NotificationSettingsUpdatedEvent(
    int UserId,
    bool EmailNotificationsEnabled,
    bool PushNotificationsEnabled,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};