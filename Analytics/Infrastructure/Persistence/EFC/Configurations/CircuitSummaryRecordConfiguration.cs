using Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Configurations;

public class CircuitSummaryRecordConfiguration : IEntityTypeConfiguration<CircuitSummaryRecord>
{
    public void Configure(EntityTypeBuilder<CircuitSummaryRecord> builder)
    {
        builder.ToTable("analytics_circuit_summary_records");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");

        builder.Property(r => r.DashboardId)
            .HasColumnName("dashboard_id");

        builder.Property(r => r.CircuitId)
            .HasColumnName("circuit_id");

        builder.Property(r => r.TotalKilowattHours)
            .HasColumnName("total_kwh");

        builder.Property(r => r.PeakVoltage)
            .HasColumnName("peak_voltage");

        builder.Property(r => r.PeakCurrent)
            .HasColumnName("peak_current");

        builder.Property(r => r.LastReadingAt)
            .HasColumnName("last_reading_at");

        builder.HasIndex(r => new { r.DashboardId, r.CircuitId }).IsUnique();
    }
}
