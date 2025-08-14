using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;

public record TechnicianCoverageAreaUpdatedEvent(
    int ProfileId,
    string NewCoverageAreaDetails,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};