using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Processing.Domain.Repositories;
using Hampcoders.Electrolink.API.Processing.Domain.Services;

namespace Hampcoders.Electrolink.API.Processing.Application.Internal.QueryServices;

public class IoTMonitoringQueryService(
    IDeviceReadingStreamRepository streamRepository,
    IAnomalyRecordRepository       anomalyRepository,
    IRelayControlCommandRepository relayRepository)
    : IIoTMonitoringQueryService
{
    public async Task<DeviceReadingStream?> Handle(GetDeviceStatusQuery query)
        => await streamRepository.FindByDeviceIdAsync(query.DeviceId);

    public async Task<IEnumerable<AnomalyRecord>> Handle(GetActiveAnomaliesByPropertyQuery query)
        => await anomalyRepository.FindActiveByPropertyAsync(query.PropertyId);

    public async Task<DeviceReadingStream?> Handle(GetRecentReadingsWindowQuery query)
        => await streamRepository.FindByDeviceIdAsync(query.DeviceId);

    public async Task<IEnumerable<RelayControlCommand>> Handle(GetRelayCommandHistoryQuery query)
        => await relayRepository.FindHistoryByDeviceAsync(query.DeviceId);

    public async Task<RelayControlCommand?> FindPendingRelayCommandByDeviceAsync(string deviceId)
        => await relayRepository.FindPendingByDeviceAsync(deviceId);
}
