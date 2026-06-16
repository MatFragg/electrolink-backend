using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record RelayCommandExecutedEvent(
    string   CommandId,
    string   DeviceId,
    string   PropertyId,
    string   FinalRelayState,
    string   RequestedBy,
    string?  ServiceRequestId,
    DateTime ExecutedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => ExecutedAt;
}
