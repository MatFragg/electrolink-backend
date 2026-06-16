using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Configurations;

public class TechnicianMetricsConfiguration : IEntityTypeConfiguration<TechnicianMetrics>
{
    public void Configure(EntityTypeBuilder<TechnicianMetrics> builder)
    {
        builder.ToTable("analytics_technician_metrics");

        builder.HasKey(m => m.MetricsId);
        builder.Property(m => m.MetricsId)
            .HasConversion(id => id.Value, v => TechnicianMetricsId.From(v))
            .HasColumnName("metrics_id");

        builder.Property(m => m.TechnicianId)
            .HasConversion(id => id.Value, v => TechnicianId.From(v))
            .HasColumnName("technician_id");

        builder.OwnsOne(m => m.Period, p =>
        {
            p.WithOwner().HasForeignKey("MetricsId");
            p.Property(r => r.Start).HasColumnName("period_start");
            p.Property(r => r.End).HasColumnName("period_end");
        });

        builder.Property(m => m.CompletedServicesCount).HasColumnName("completed_services_count");
        builder.Property(m => m.IoTServicesCount).HasColumnName("iot_services_count");
        builder.Property(m => m.AverageRating).HasColumnName("average_rating");

        builder.OwnsOne(m => m.TotalRevenue, r =>
        {
            r.WithOwner().HasForeignKey("MetricsId");
            r.Property(p => p.Amount).HasColumnName("total_revenue_amount");
            r.Property(p => p.Currency).HasColumnName("total_revenue_currency");
        });

        builder.Property(m => m.AverageResponseTime).HasColumnName("average_response_time");
        builder.Property(m => m.ClientRetentionRate).HasColumnName("client_retention_rate");
        builder.Property(m => m.LastCalculatedAt).HasColumnName("last_calculated_at");

        builder.Ignore(m => m.DomainEvents);
    }
}
