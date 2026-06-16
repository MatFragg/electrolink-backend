using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface ITechnicianMetricsQueryService
{
    Task<TechnicianMetrics?> GetPerformanceDashboardAsync(string technicianId);
}
