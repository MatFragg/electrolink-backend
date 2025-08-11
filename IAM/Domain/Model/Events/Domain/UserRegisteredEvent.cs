using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;

public record UserRegisteredEvent(
    int UserId,
    string Username,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};