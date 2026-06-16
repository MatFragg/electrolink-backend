using Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Processing.Domain.Repositories;
using Hampcoders.Electrolink.API.Processing.Domain.Services;
using Hampcoders.Electrolink.API.Processing.Interfaces.ACL;
using Hampcoders.Electrolink.API.Processing.Application.Internal.OutboundServices;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.ACL.Services;

public class ProcessingContextFacade(
    IRelayCommandService       relayService,
    IIoTMonitoringQueryService queryService,
    IDeviceReadingStreamRepository streamRepository,
    IAnomalyRecordRepository   anomalyRepository,
    IServiceOperationContextFacade serviceOpFacade)
    : IProcessingContextFacade
{
    public async Task RequestRelayToggleAsync(
        string executionId, string deviceId, string technicianId,
        string relayState, string reason, DateTime requestedAt)
    {
        var stream = await streamRepository.FindByDeviceIdAsync(deviceId);
        if (stream is null)
            throw new KeyNotFoundException($"DeviceReadingStream not found for device {deviceId}.");

        await relayService.Handle(new IssueRelayCommandCommand(
            deviceId, stream.PropertyId.Value,
            technicianId, executionId, relayState));
    }

    public async Task<IoTContextSnapshotDto?> GetIoTContextSnapshotAsync(string propertyId)
    {
        var stream = await streamRepository.FindByPropertyIdAsync(propertyId);
        if (stream is null) return null;

        var activeAnomalies = await anomalyRepository.FindActiveByPropertyAsync(propertyId);
        var recentAnomalies = await anomalyRepository.FindRecentByPropertyAsync(propertyId, 7);

        return new IoTContextSnapshotDto(
            DeviceId: stream.DeviceId.Value,
            DeviceStatus: "INSTALLED",
            StreamStatus: stream.StreamStatus.ToString(),
            CapturedAt: DateTime.UtcNow,
            RecentReadings: stream.Readings
                .OrderByDescending(r => r.Timestamp)
                .Take(10)
                .Select(r => new ReadingEntryDto(
                    r.Timestamp, r.Voltage, r.Current, r.PowerFactor, r.Frequency))
                .ToList(),
            ActiveAnomalies: activeAnomalies
                .Select(a => new AnomalyEntryDto(
                    a.AnomalyId.Value, a.AnomalyType.ToString(),
                    a.Severity.ToString(), a.DetectedAt))
                .ToList());
    }

    public async Task<bool> HasActiveServiceForPropertyAsync(string propertyId, string technicianId)
        => await serviceOpFacade.HasActiveServiceForPropertyAsync(propertyId, technicianId);
}
