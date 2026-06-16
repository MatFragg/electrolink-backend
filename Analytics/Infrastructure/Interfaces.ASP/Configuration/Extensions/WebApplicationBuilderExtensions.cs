using Hampcoders.Electrolink.API.Analytics.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Analytics.Application.Internal.Services;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Analytics.Infrastructure.Jobs;
using Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Services;
using Hampcoders.Electrolink.API.Analytics.Interfaces.ACL;
using Hampcoders.Electrolink.API.Analytics.Interfaces.ACL.Services;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddAnalyticsContextServices(this WebApplicationBuilder builder)
    {
        // Repositories
        builder.Services.AddScoped<IConsumptionDashboardRepository, ConsumptionDashboardRepository>();
        builder.Services.AddScoped<IAlertLogRepository, AlertLogRepository>();
        builder.Services.AddScoped<ITechnicianMetricsRepository, TechnicianMetricsRepository>();
        builder.Services.AddScoped<IConsumptionReportRepository, ConsumptionReportRepository>();

        // Command Services
        builder.Services.AddScoped<IConsumptionDashboardCommandService, ConsumptionDashboardCommandService>();
        builder.Services.AddScoped<IAlertLogCommandService, AlertLogCommandService>();
        builder.Services.AddScoped<ITechnicianMetricsCommandService, TechnicianMetricsCommandService>();
        builder.Services.AddScoped<IConsumptionReportCommandService, ConsumptionReportCommandService>();

        // Query Services
        builder.Services.AddScoped<IConsumptionDashboardQueryService, ConsumptionDashboardQueryService>();
        builder.Services.AddScoped<IAlertLogQueryService, AlertLogQueryService>();
        builder.Services.AddScoped<ITechnicianMetricsQueryService, TechnicianMetricsQueryService>();
        builder.Services.AddScoped<IConsumptionReportQueryService, ConsumptionReportQueryService>();

        // ACL Facade
        builder.Services.AddScoped<IAnalyticsContextFacade, AnalyticsContextFacade>();

        // Projection Services
        builder.Services.AddScoped<IDashboardProjectionService, DashboardProjectionService>();

        // Jobs
        builder.Services.AddScoped<CostProjectionSchedulerJob>();
        builder.Services.AddScoped<TechnicianMetricsPeriodInitializerJob>();
    }
}
