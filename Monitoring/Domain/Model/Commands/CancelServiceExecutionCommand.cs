using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to cancel a service execution.
/// </summary>
public record CancelServiceExecutionCommand(
    ServiceExecutionId ExecutionId,
    string ActorId,
    ECancelledBy CancelledBy,
    string Reason,
    string? Notes,
    bool RequestReassignment,
    CancellationRequestId? CancellationRequestId = null
);
