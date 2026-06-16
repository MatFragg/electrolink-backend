using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;

public class TechnicianMetricsQueryService(
    ITechnicianMetricsRepository metricsRepository)
    : ITechnicianMetricsQueryService
{
    public async Task<TechnicianMetrics?> GetPerformanceDashboardAsync(string technicianId)
    {
        return await metricsRepository.FindCurrentPeriodByTechnicianAsync(
            TechnicianId.From(technicianId));
    }
}
