using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceDecommissionedEvent(
    string DeviceId,
    string SerialNumber,
    string? PropertyId,
    EDeviceStatus PreviousStatus,
    string Reason,
    DateTime DecommissionedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => DecommissionedAt;
}
