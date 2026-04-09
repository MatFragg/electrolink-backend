using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to record actual component usage during service.
/// </summary>
public record RecordComponentsUsedCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    IReadOnlyList<ComponentUsageItem> ComponentsUsed,
    DateTime RecordedAt
);

