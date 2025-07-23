using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Components;

public record ComponentUpdatedEvent(
    ComponentId ComponentId,
    string NewName,
    string NewDescription,
    string NewManufacturer,
    decimal NewUnitCost,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};