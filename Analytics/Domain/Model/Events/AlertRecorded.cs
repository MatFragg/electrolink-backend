using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record AlertRecorded(
    string LogId,
    string EntryId,
    string HomeownerId,
    string AlertType,
    string Severity,
    string? CircuitId,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
