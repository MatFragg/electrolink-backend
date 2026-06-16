using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record AnomalyResolvedEvent(
    string   AnomalyId,
    string   DeviceId,
    string   PropertyId,
    string   ResolvedByMethod,
    DateTime ResolvedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => ResolvedAt;
}
