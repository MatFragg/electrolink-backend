using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record ServiceDetailsAddedEvent(
    Guid RequestId,
    Guid RecipeId,
    Guid TechnicianId,
    bool IsPriority,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

