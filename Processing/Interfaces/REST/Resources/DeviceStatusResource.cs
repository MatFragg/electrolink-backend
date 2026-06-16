namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record DeviceStatusResource(
    string   DeviceId,
    string   PropertyId,
    string   ConnectionStatus,
    string   StreamStatus,
    DateTime LastReadingAt,
    ReadingValueResource? LastReading,
    IReadOnlyList<ActiveAnomalySummaryResource> ActiveAnomalies,
    string   CurrentRelayState
);

public record ReadingValueResource(
    float    Voltage,
    float    Current,
    float    PowerFactor,
    float    Frequency,
    DateTime Timestamp
);

public record ActiveAnomalySummaryResource(
    string   AnomalyId,
    string   AnomalyType,
    string   Severity,
    DateTime DetectedAt
);
