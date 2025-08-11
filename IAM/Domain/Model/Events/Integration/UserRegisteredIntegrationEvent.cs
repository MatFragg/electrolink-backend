using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Integration;

public record UserRegisteredIntegrationEvent(
    int UserId,
    string Username,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};