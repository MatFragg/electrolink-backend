using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Integration;

public record UsernameUpdatedIntegrationEvent(
    int UserId,
    string NewUsername,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
