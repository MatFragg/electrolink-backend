using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        // ServiceOperation aggregate
        builder.Entity<ServiceOperation>(b =>
        {
            b.HasKey(o => o.RequestId);

            b.Property(o => o.CurrentStatus)
                .HasConversion<string>()
                .IsRequired();

            b.Property(o => o.TechnicianId)
                .HasConversion(
                    id => id.Value,
                    value => new TechnicianId(value)
                )
                .HasColumnName("TechnicianId")
                .IsRequired();

            b.OwnsMany(o => o.StatusHistory, sh =>
            {
                sh.WithOwner().HasForeignKey("RequestId");
                sh.HasKey("RequestId", nameof(RequestStatus.Timestamp));
                sh.Property(s => s.Status).HasConversion<string>().IsRequired();
                sh.Property(s => s.Timestamp).IsRequired();
            });

            b.Navigation(o => o.StatusHistory).AutoInclude();
        });

        builder.Entity<Report>(b =>
        {
            b.HasKey(r => r.ReportId);

            b.Property(r => r.Description)
                .IsRequired();

            b.Property(r => r.Date)
                .IsRequired();

            b.HasIndex(r => r.RequestId)
                .IsUnique();
            
        });

        builder.Entity<ReportPhoto>(b =>
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.Url).IsRequired();
            b.Property(p => p.Type).IsRequired();
            b.Property(p => p.ReportId).IsRequired();

            b.HasIndex(p => p.ReportId);
        });


        // Rating aggregate
        builder.Entity<Rating>(b =>
        {
            b.HasKey(r => r.RatingId);

            b.Property(r => r.Score).IsRequired();
            b.Property(r => r.Comment).IsRequired();
            b.Property(r => r.RaterId).IsRequired();
            b.Property(r => r.TechnicianId)
                .HasConversion(
                    id => id.Value,
                    value => new TechnicianId(value)
                )
                .HasColumnName("TechnicianId")
                .IsRequired();

            b.HasIndex(r => r.RequestId).IsUnique();
        });
    }
}
