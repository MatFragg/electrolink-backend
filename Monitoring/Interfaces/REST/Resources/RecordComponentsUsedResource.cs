namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record RecordComponentsUsedResource(
    IReadOnlyList<ComponentUsageItemResource> ComponentsUsed,
    DateTime RecordedAt
);