using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Configurations.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ConsumptionDashboardConfiguration());
        builder.ApplyConfiguration(new AlertLogConfiguration());
        builder.ApplyConfiguration(new TechnicianMetricsConfiguration());
        builder.ApplyConfiguration(new ConsumptionReportConfiguration());
        builder.ApplyConfiguration(new TimeSeriesRecordConfiguration());
        builder.ApplyConfiguration(new CircuitSummaryRecordConfiguration());
    }
}
