using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Jobs;

public class TechnicianMetricsPeriodInitializerJob(
    ITechnicianMetricsCommandService metricsCommandService,
    ITechnicianMetricsRepository metricsRepository)
{
    private readonly ITechnicianMetricsCommandService _metricsCommandService = metricsCommandService;
    private readonly ITechnicianMetricsRepository _metricsRepository = metricsRepository;

    public async Task ExecuteAsync(List<string> activeTechnicianIds)
    {
        foreach (var technicianId in activeTechnicianIds)
        {
            await _metricsCommandService.InitializeTechnicianMetricsPeriodAsync(technicianId);
        }
    }
}
