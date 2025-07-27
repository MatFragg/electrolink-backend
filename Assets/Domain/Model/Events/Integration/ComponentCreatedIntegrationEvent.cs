using System.Text.Json;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;

public record ComponentCreatedIntegrationEvent(
    ComponentId ComponentId,
    ComponentTypeId ComponentTypeId,
    string ComponentName,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; } = nameof(ComponentCreatedIntegrationEvent);
    public string Payload { get; init; } = JsonSerializer.Serialize(new { ComponentId = ComponentId.Id, ComponentTypeId = ComponentTypeId.Id, ComponentName, OccurredOn });
};