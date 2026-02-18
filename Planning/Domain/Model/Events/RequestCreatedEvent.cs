using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events;

public record RequestCreatedEvent(
    RequestId RequestId,
    ClientId ClientId,
    PropertyId PropertyId,
    ServiceId ServiceId,
    RequestPriority Priority,
    DateOnly ScheduledDate,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}