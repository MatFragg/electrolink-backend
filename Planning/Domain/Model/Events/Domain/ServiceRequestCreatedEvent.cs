using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record ServiceRequestCreatedEvent(
    Guid RequestId,
    Guid HomeownerId,
    Guid PropertyId,
    double PropertyLatitude,
    double PropertyLongitude,
    Guid SelectedRecipeId,
    Guid SelectedTechnicianId,
    bool IsPriority,
    List<DateTime> PreferredDates,
    TimePreference TimePreference,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

