using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when a reassignment is requested due to cancellation.
/// </summary>
public record ReassignmentRequestedEvent(
    ServiceExecutionId ExecutionId,
    RequestId RequestId,
    DateTime OccurredOn
);

