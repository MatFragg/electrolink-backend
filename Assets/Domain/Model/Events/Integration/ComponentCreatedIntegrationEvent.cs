using System.Text.Json;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;

public record ComponentCreatedIntegrationEvent(
    ComponentId ComponentId,
    ComponentTypeId ComponentTypeId,
    string ComponentName,
    DateTime OccurredOn,
    Guid EventId
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};