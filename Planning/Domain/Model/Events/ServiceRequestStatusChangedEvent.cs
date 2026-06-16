using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events;

public record ServiceRequestStatusChangedEvent(
    RequestId RequestId,
    ERequestStatus FromStatus,
    ERequestStatus ToStatus,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
