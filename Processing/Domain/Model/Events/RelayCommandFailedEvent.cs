using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record RelayCommandFailedEvent(
    string   CommandId,
    string   DeviceId,
    string   PropertyId,
    string   TargetRelayState,
    string   FailureReason,
    DateTime OccurredAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => OccurredAt;
}
