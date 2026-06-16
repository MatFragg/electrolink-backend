using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceInstalledEvent(
    string DeviceId,
    string SerialNumber,
    string PropertyId,
    string InstalledByTechnicianId,
    string InstallationRequestId,
    string FirmwareVersion,
    DateTime InstalledAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => InstalledAt;
}
