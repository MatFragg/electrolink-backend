namespace Hampcoders.Electrolink.API.Processing.Interfaces.ACL.Services;

public record IoTContextSnapshotDto(
    string DeviceId,
    string DeviceStatus,
    string StreamStatus,
    DateTime CapturedAt,
    IReadOnlyList<ReadingEntryDto> RecentReadings,
    IReadOnlyList<AnomalyEntryDto> ActiveAnomalies);

public record ReadingEntryDto(DateTime Timestamp, float Voltage, float Current, float PowerFactor, float Frequency);

public record AnomalyEntryDto(string AnomalyId, string AnomalyType, string Severity, DateTime DetectedAt);
