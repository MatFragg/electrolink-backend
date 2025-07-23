using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.TechnicianInventories;

public record ComponentStockIncreasedEvent(
    Guid ComponentStockId,
    ComponentId ComponentId,
    int IncreasedQuantity, // <--- Asegúrate que sea int, no int?
    int NewQuantityAvailable, // <--- Asegúrate que sea int, no int?
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};