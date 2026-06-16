using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record AnomalyAcknowledgedEvent(
    string   AnomalyId,
    string   DeviceId,
    string   PropertyId,
    DateTime AcknowledgedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => AcknowledgedAt;
}
