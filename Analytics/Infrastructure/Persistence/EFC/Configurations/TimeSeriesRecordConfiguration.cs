using Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Configurations;

public class TimeSeriesRecordConfiguration : IEntityTypeConfiguration<TimeSeriesRecord>
{
    public void Configure(EntityTypeBuilder<TimeSeriesRecord> builder)
    {
        builder.ToTable("analytics_time_series_records");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");

        builder.Property(r => r.DashboardId)
            .HasColumnName("dashboard_id");

        builder.Property(r => r.Timestamp)
            .HasColumnName("timestamp");

        builder.Property(r => r.KilowattHours)
            .HasColumnName("kilowatt_hours");

        builder.Property(r => r.Granularity)
            .HasColumnName("granularity");

        builder.HasIndex(r => new { r.DashboardId, r.Timestamp, r.Granularity }).IsUnique();
    }
}
