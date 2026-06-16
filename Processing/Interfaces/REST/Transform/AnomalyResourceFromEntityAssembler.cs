using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Transform;

public static class AnomalyResourceFromEntityAssembler
{
    public static AnomalyResource ToResourceFromEntity(AnomalyRecord anomaly) =>
        new(
            anomaly.AnomalyId.Value,
            anomaly.DeviceId.Value,
            anomaly.PropertyId.Value,
            anomaly.AnomalyType.ToString(),
            anomaly.Severity.ToString(),
            anomaly.DetectionLayer.ToString(),
            anomaly.Status.ToString(),
            anomaly.DetectedAt,
            anomaly.ResolvedAt,
            anomaly.AutoRelayActivated);
}
