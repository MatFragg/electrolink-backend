using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceAssignedToPropertyEvent(
    string DeviceId,
    string SerialNumber,
    string PropertyId,
    string InstallationRequestId,
    DateTime AssignedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => AssignedAt;
}
