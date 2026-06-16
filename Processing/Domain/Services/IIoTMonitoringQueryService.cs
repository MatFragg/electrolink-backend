using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Processing.Domain.Services;

public interface IIoTMonitoringQueryService
{
    Task<DeviceReadingStream?> Handle(GetDeviceStatusQuery query);
    Task<IEnumerable<AnomalyRecord>> Handle(GetActiveAnomaliesByPropertyQuery query);
    Task<DeviceReadingStream?> Handle(GetRecentReadingsWindowQuery query);
    Task<IEnumerable<RelayControlCommand>> Handle(GetRelayCommandHistoryQuery query);
    Task<RelayControlCommand?> FindPendingRelayCommandByDeviceAsync(string deviceId);
}
