using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceSentToMaintenanceEvent(
    string DeviceId,
    string PropertyId,
    string Reason,
    DateTime? ExpectedReturnDate,
    DateTime SentAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => SentAt;
}
