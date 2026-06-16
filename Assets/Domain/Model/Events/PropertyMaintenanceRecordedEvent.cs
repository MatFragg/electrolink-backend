using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record PropertyMaintenanceRecordedEvent(PropertyId PropertyId, AssignmentId AssignmentId, TechnicianId TechnicianId, string WorkSummary, DateTime CompletedAt, DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}