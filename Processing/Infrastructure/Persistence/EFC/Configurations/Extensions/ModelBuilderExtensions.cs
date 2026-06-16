using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProcessingConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new Configurations.DeviceReadingStreamConfiguration());
        builder.ApplyConfiguration(new Configurations.ReadingConfiguration());
        builder.ApplyConfiguration(new Configurations.AnomalyRecordConfiguration());
        builder.ApplyConfiguration(new Configurations.RelayControlCommandConfiguration());
    }
}
