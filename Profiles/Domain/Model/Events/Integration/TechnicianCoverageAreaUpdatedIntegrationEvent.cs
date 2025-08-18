using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;

public record TechnicianCoverageAreaUpdatedIntegrationEvent(
    Guid TechnicianId,
    string NewCoverageAreaDetails,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
