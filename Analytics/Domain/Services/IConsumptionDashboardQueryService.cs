using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IConsumptionDashboardQueryService
{
    Task<DashboardView?> GetDashboardViewAsync(string homeownerId);
    Task<DashboardView?> GetCostProjectionAsync(string homeownerId);
    Task<DashboardView?> GetRealTimeCircuitMonitorAsync(string homeownerId);
}
