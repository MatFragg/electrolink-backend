using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public record ServiceCompletedExternalEvent(
    string ServiceId,
    string ServiceType,
    string PropertyId,
    string TechnicianId,
    string? IoTDeviceId,
    string? FirmwareVersion,
    DateTime CompletedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => CompletedAt;
}
