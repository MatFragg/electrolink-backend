using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when the technical report is updated.
/// </summary>
public record TechnicalReportUpdatedEvent(
    ServiceExecutionId ExecutionId,
    int Version,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};

