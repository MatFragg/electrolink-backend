using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when component usage is recorded.
/// </summary>
public record ComponentsActuallyUsedRecordedEvent(
    ServiceExecutionId ExecutionId,
    AssignmentId AssignmentId,
    IReadOnlyList<ComponentUsage> Components,
    bool HasOverage,
    DateTime RecordedAt,
    DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}

