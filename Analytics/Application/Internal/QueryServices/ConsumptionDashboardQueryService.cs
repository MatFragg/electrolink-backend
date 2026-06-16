using Hampcoders.Electrolink.API.Analytics.Application.Internal.Services;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;

public class ConsumptionDashboardQueryService(
    IConsumptionDashboardRepository dashboardRepository,
    IDashboardProjectionService projectionService)
    : IConsumptionDashboardQueryService
{
    public async Task<DashboardView?> GetDashboardViewAsync(string homeownerId)
    {
        var dashboard = await dashboardRepository.FindByHomeownerIdAsync(HomeownerId.From(homeownerId));
        if (dashboard == null) return null;

        var timeSeries = await projectionService.GetTimeSeriesAsync(dashboard.DashboardId.Value);
        var circuitSummaries = await projectionService.GetCircuitSummariesAsync(dashboard.DashboardId.Value);

        return new DashboardView(dashboard, timeSeries, circuitSummaries);
    }

    public async Task<DashboardView?> GetCostProjectionAsync(string homeownerId)
    {
        var dashboard = await dashboardRepository.FindByHomeownerIdAsync(HomeownerId.From(homeownerId));
        if (dashboard == null) return null;

        var timeSeries = await projectionService.GetTimeSeriesAsync(dashboard.DashboardId.Value);

        return new DashboardView(dashboard, timeSeries, []);
    }

    public async Task<DashboardView?> GetRealTimeCircuitMonitorAsync(string homeownerId)
    {
        var dashboard = await dashboardRepository.FindByHomeownerIdAsync(HomeownerId.From(homeownerId));
        if (dashboard == null) return null;

        var circuitSummaries = await projectionService.GetCircuitSummariesAsync(dashboard.DashboardId.Value);

        return new DashboardView(dashboard, [], circuitSummaries);
    }
}
