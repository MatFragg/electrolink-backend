using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;

public record UsernameUpdatedEvent(
    int UserId,
    string OldUsername,
    string NewUsername,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};