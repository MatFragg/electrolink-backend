using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events;

public record TechnicianAssignedEvent(
    RequestId RequestId,
    TechnicianId TechnicianId,
    string ClientId,
    string ServiceId,
    DateOnly ScheduledDate,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}