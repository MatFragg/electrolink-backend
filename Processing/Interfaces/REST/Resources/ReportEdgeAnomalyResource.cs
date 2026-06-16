namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record ReportEdgeAnomalyResource(
    string DeviceId,
    string AnomalyType,
    string TriggerReadingId,
    string EdgeAlertPayloadJson
);
