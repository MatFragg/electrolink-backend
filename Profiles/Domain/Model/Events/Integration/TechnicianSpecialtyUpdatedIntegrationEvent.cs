using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;

public record TechnicianSpecialtyUpdatedIntegrationEvent(
    Guid TechnicianId,
    IReadOnlyList<string> NewSpecialties,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
