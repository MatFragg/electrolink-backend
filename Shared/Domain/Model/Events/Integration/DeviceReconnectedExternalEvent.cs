using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public record DeviceReconnectedExternalEvent(
    string DeviceId,
    DateTime ReconnectedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => ReconnectedAt;
}
