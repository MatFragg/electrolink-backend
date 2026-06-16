using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceReinstalledEvent(
    string DeviceId,
    string PropertyId,
    string ReinstalledByTechnicianId,
    string FirmwareVersion,
    DateTime ReinstalledAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => ReinstalledAt;
}
