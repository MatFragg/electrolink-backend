using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record IoTDeviceRegisteredEvent(
    string DeviceId,
    string SerialNumber,
    string FirmwareVersion,
    DateTime RegisteredAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => RegisteredAt;
}
