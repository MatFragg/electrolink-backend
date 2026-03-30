using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events;

public record ServiceCatalogStatusChangedEvent(
    CatalogId CatalogId,
    ECatalogStatus PreviousStatus,
    ECatalogStatus NewStatus,
    DateTime OccurredOn
) : IEvent {
    public Guid EventId { get; init; } = Guid.NewGuid();
}
