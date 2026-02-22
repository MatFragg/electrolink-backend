using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public record ProfileCompletedEvent(
    string ProfileId,
    string UserId,
    string TechnicianId, 
    string BusinessRole,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}