using System.Text.Json;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Integration;

public record ComponentStockLowIntegrationEvent(
    ComponentId ComponentId,
    int CurrentQuantity,
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; } = nameof(ComponentStockLowIntegrationEvent);
    public string Payload { get; init; } = JsonSerializer.Serialize(new { ComponentId = ComponentId.Id, CurrentQuantity, OccurredOn });
};