using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using TechnicianId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.TechnicianId;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record TechnicianInventoryCreatedEvent(TechnicianInventoryId TechnicianInventoryId, TechnicianId TechnicianId, DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};