using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Repositories;

public interface ITechnicianMetricsRepository : IBaseRepository<TechnicianMetrics, TechnicianMetricsId>
{
    Task<TechnicianMetrics?> FindByTechnicianAndPeriodAsync(TechnicianId technicianId, DateRange period);
    Task<TechnicianMetrics?> FindCurrentPeriodByTechnicianAsync(TechnicianId technicianId);
}
