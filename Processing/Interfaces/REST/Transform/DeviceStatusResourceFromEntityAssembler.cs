using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Transform;

public static class DeviceStatusResourceFromEntityAssembler
{
    public static DeviceStatusResource ToResourceFromEntity(
        DeviceReadingStream stream,
        IEnumerable<AnomalyRecord> activeAnomalies,
        string currentRelayState)
    {
        var lastReading = stream.Readings
            .OrderByDescending(r => r.Timestamp)
            .FirstOrDefault();

        return new DeviceStatusResource(
            stream.DeviceId.Value,
            stream.PropertyId.Value,
            stream.StreamStatus == EStreamStatus.Inactive ? "DISCONNECTED" : "CONNECTED",
            stream.StreamStatus.ToString(),
            stream.LastReceivedAt,
            lastReading is null ? null : new ReadingValueResource(
                lastReading.Voltage, lastReading.Current,
                lastReading.PowerFactor, lastReading.Frequency,
                lastReading.Timestamp),
            activeAnomalies.Select(a => new ActiveAnomalySummaryResource(
                a.AnomalyId.Value, a.AnomalyType.ToString(),
                a.Severity.ToString(), a.DetectedAt)).ToList(),
            currentRelayState);
    }
}
