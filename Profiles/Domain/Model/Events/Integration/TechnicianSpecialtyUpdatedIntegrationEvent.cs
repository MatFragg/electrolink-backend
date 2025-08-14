using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;

public record TechnicianSpecialtyUpdatedIntegrationEvent(
    int ProfileId,
    IReadOnlyList<string> NewSpecialties,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
