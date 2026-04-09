using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when a no-show incident is officially recorded.
/// </summary>
public record TechnicianNoShowIncidentRecordedEvent(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    DateTime OccurredOn
);

