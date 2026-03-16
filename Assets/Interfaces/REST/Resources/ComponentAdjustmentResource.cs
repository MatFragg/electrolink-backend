namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Nested resource for a component adjustment (componentId + quantity).
/// Used inside ReserveComponentsResource.
/// </summary>
public record ComponentAdjustmentResource(string ComponentId, int Quantity);

