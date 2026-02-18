using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record PropertySelectedForRequestEvent(
    Guid RequestId,
    Guid PropertyId,
    string Address,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

