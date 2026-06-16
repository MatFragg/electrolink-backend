using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;

public record ReportRequested(
    string ReportId,
    string HomeownerId,
    string PropertyId,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
