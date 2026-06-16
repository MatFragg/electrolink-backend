using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public record IoTInstallationServiceScheduledExternalEvent(
    string PropertyId,
    string InstallationServiceRequestId,
    string SelectedDeviceId,
    DateTime ScheduledAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => ScheduledAt;
}
