using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record BufferedReadingsReplayedEvent(
    string   DeviceId,
    int      ReadingsCount,
    int      ValidReadingsCount,
    DateTime OccurredAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => OccurredAt;
}
