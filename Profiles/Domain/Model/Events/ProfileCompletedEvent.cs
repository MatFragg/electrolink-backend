using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public record ProfileCompletedEvent(
    string ProfileId,
    string UserId,
    object SubjectId, 
    EBusinessRole BusinessRole,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}