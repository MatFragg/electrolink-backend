namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource for reserving components for a service.
/// </summary>
public record ReserveComponentsResource(
    string ServiceId,
    List<ComponentAdjustmentResource> Components);

