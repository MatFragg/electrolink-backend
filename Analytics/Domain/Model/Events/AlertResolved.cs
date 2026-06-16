using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record AlertResolved(
    string LogId,
    string EntryId,
    string HomeownerId,
    string SourceEventId,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
