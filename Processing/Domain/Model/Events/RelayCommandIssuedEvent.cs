using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Events;

public record RelayCommandIssuedEvent(
    string   CommandId,
    string   DeviceId,
    string   PropertyId,
    string   TargetRelayState,
    string   RequestedBy,
    string   AuthorizationSource,
    string?  ServiceRequestId,
    DateTime IssuedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    DateTime IEvent.OccurredOn => IssuedAt;
}
