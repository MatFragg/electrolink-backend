using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ServiceExecutionConfiguration());
        builder.ApplyConfiguration(new ServiceCancellationRequestConfiguration());
    }
}
