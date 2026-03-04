using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record ComponentTypeUpdatedEvent(ComponentTypeId ComponentTypeId, string Name, string? Description, DateTime OccurredOn) : IEvent 
{ 
public Guid EventId { get; init; } = Guid.NewGuid();
}