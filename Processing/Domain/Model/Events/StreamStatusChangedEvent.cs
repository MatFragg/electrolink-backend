using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record StreamStatusChangedEvent(
    string   StreamId,
    string   DeviceId,
    string   PreviousStatus,
    string   NewStatus,
    DateTime OccurredAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => OccurredAt;
}
