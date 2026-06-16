using Hampcoders.Electrolink.API.Monitoring.Application.ACL;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddMonitoringServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IServiceExecutionCommandService, ServiceExecutionCommandService>();
        builder.Services.AddScoped<IServiceExecutionQueryService, ServiceExecutionQueryService>();
        builder.Services.AddScoped<IServiceExecutionRepository, ServiceExecutionRepository>();
        builder.Services.AddScoped<IMonitoringContextFacade, MonitoringContextFacade>();
        builder.Services.AddScoped<IServiceCancellationRequestRepository, ServiceCancellationRequestRepository>();

        builder.Services.AddScoped<IExternalAssetsService, ExternalAssetsService>();
        builder.Services.AddScoped<IExternalMonitoringService, ExternalMonitoringService>();
        builder.Services.AddScoped<IExternalAnalyticsService, ExternalAnalyticsService>();
        builder.Services.AddScoped<IExternalIoTService, ExternalIoTService>();
    }
}
