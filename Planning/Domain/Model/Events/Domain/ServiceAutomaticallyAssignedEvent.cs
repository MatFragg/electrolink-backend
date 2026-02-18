using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record ServiceAutomaticallyAssignedEvent(
    Guid ServiceId,
    Guid RequestId,
    Guid TechnicianId,
    Guid HomeownerId,
    Guid PropertyId,
    RecipeSnapshot RecipeSnapshot,
    DateTime ScheduledStartDateTime,
    DateTime ScheduledEndDateTime,
    bool IsPriority,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

