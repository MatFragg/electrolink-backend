using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;


public record ProfileUpdatedEvent(
    int ProfileId,
    string OldFullName,
    string NewFullName,
    string OldEmailAddress,
    string NewEmailAddress,
    string OldStreetAddress,
    string NewStreetAddress,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};