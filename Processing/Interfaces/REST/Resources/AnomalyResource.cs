namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record AnomalyResource(
    string   AnomalyId,
    string   DeviceId,
    string   PropertyId,
    string   AnomalyType,
    string   Severity,
    string   DetectionLayer,
    string   Status,
    DateTime DetectedAt,
    DateTime? ResolvedAt,
    bool     AutoRelayActivated
);
