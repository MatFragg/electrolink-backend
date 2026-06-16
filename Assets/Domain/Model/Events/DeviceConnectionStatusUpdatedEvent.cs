using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record DeviceConnectionStatusUpdatedEvent(
    string DeviceId,
    string PropertyId,
    EConnectionStatus PreviousStatus,
    EConnectionStatus NewStatus,
    DateTime? LastReadingAt,
    DateTime UpdatedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn => UpdatedAt;
}
