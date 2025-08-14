using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;

public record ProfileUpdatedIntegrationEvent(
    int ProfileId,
    string NewFullName,
    string NewEmailAddress,
    string NewStreetAddress,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
