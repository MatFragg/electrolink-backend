using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public record DeviceDisconnectedExternalEvent(
    string DeviceId,
    DateTime? LastReadingAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
