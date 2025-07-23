using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;

public record ComponentStockDecreasedEvent(Guid ComponentStockId, ComponentId ComponentId, int DecreasedQuantity, int NewQuantityAvailable, DateTime OccurredOn) : IEvent{
    public Guid EventId { get; init; } = Guid.NewGuid();
}