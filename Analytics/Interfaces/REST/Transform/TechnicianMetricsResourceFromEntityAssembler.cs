using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Transform;

public static class TechnicianMetricsResourceFromEntityAssembler
{
    public static TechnicianMetricsResource ToResourceFromEntity(TechnicianMetrics metrics)
    {
        return new TechnicianMetricsResource(
            metrics.MetricsId.Value,
            metrics.TechnicianId.Value,
            metrics.Period.Start,
            metrics.Period.End,
            metrics.CompletedServicesCount,
            metrics.IoTServicesCount,
            metrics.AverageRating,
            metrics.TotalRevenue.Amount,
            metrics.TotalRevenue.Currency,
            metrics.AverageResponseTime,
            metrics.ClientRetentionRate,
            metrics.LastCalculatedAt);
    }
}
