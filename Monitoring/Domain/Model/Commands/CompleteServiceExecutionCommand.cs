using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to mark a service execution as complete.
/// </summary>
public record CompleteServiceExecutionCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    DateTime CompletedAt
);


