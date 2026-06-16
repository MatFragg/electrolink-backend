namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IAlertLogCommandService
{
    Task RecordAnomalyAlertAsync(string homeownerId, string anomalyEventId, string severity, string? circuitId);
    Task RecordThresholdAlertAsync(string homeownerId, string circuitId, decimal consumedKWh, string readingEventId);
    Task RecordDeviceDisconnectionAlertAsync(string homeownerId, string deviceEventId, string? circuitId);
    Task RecordCircuitToggleAlertAsync(string homeownerId, string relayEventId, string circuitId);
    Task ResolveAlertAsync(string homeownerId, string sourceEventId);
    Task AcknowledgeAlertAsync(string homeownerId, string entryId);
    Task LinkAlertToServiceRequestAsync(string homeownerId, string entryId, string serviceRequestId);
}
