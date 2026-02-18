using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyServiceDesignAndPlanningConfiguration(this ModelBuilder builder)
    {
        // --- Service Aggregate ---
        builder.Entity<Service>(b =>
        {
            b.ToTable("services");

            b.HasKey(s => s.Id);
            b.Property(s => s.Id)
                .HasConversion(new StronglyTypedIdConverter<ServiceId>())
                .HasColumnName("service_id")
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(s => s.Name).IsRequired().HasMaxLength(80);
            b.Property(s => s.Description).HasMaxLength(500);
            b.Property(s => s.Category).HasMaxLength(50);
            b.Property(s => s.EstimatedTime).IsRequired();
            b.Property(s => s.IsVisible).IsRequired();

            // BasePrice - Money Value Object
            b.OwnsOne(s => s.BasePrice, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("base_price_amount")
                    .HasPrecision(18, 2)
                    .IsRequired();
                money.Property(m => m.Currency)
                    .HasColumnName("base_price_currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            b.Property(s => s.CreatedBy)
                .HasConversion(new StronglyTypedIdConverter<TechnicianId>())
                .HasColumnName("created_by")
                .IsRequired();

            // Owned: Policy - Serializado como JSON
            b.OwnsOne(s => s.Policy, policy =>
            {
                policy.ToJson();
            });

            // Owned: Restriction - Serializado como JSON
            b.OwnsOne(s => s.Restriction, restriction =>
            {
                restriction.ToJson();
            });

            // Owned Many: Tags
            b.OwnsMany(s => s.Tags, tag =>
            {
                tag.ToJson();
            });

            // Owned Many: Components
            b.OwnsMany(s => s.Components, component =>
            {
                component.ToJson();
            });

            // Owned Many: Plans
            b.OwnsMany(s => s.Plans, plan =>
            {
                plan.ToJson();
            });

            // Owned Many: Documents
            b.OwnsMany(s => s.Documents, document =>
            {
                document.ToJson();
            });
        });

        // --- Request Aggregate ---
        builder.Entity<Request>(b =>
        {
            b.ToTable("requests");

            b.HasKey(r => r.Id);
            b.Property(r => r.Id)
                .HasConversion(new StronglyTypedIdConverter<RequestId>())
                .HasColumnName("request_id")
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(r => r.ClientId)
                .HasConversion(new StronglyTypedIdConverter<ClientId>())
                .HasColumnName("client_id")
                .IsRequired();

            b.Property(r => r.ServiceId)
                .HasConversion(new StronglyTypedIdConverter<ServiceId>())
                .HasColumnName("service_id")
                .IsRequired();

            b.Property(r => r.PropertyId)
                .HasConversion(new StronglyTypedIdConverter<PropertyId>())
                .HasColumnName("property_id")
                .IsRequired();

            b.Property(r => r.TechnicianId)
                .HasConversion(
                    v => v != null ? v.Id : (Guid?)null,
                    v => v.HasValue ? new TechnicianId(v.Value) : null)
                .HasColumnName("technician_id")
                .IsRequired(false);

            b.Property(r => r.Status)
                .HasConversion(new StronglyTypedIdConverter<RequestStatus>())
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(r => r.Priority)
                .HasConversion(new StronglyTypedIdConverter<RequestPriority>())
                .HasColumnName("priority")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(r => r.ProblemDescription).HasMaxLength(1000);
            b.Property(r => r.ScheduledDate).HasColumnName("scheduled_date");
            b.Property(r => r.CreatedDate).HasColumnName("created_date");

            // Owned: Bill - Serializado como JSON
            b.OwnsOne(r => r.Bill, bill =>
            {
                bill.ToJson();
            });

            b.OwnsMany(r => r.Photos, photo =>
            {
                photo.ToJson();
            });
        });

        // --- Schedule Entity ---
        builder.Entity<Schedule>(b =>
        {
            b.ToTable("schedules");

            b.HasKey(s => s.ScheduleId);
            b.Property(s => s.ScheduleId).HasColumnName("schedule_id").IsRequired();
            b.Property(s => s.TechnicianId).HasColumnName("technician_id").IsRequired();
            b.Property(s => s.Day).HasMaxLength(20).IsRequired();
            b.Property(s => s.StartTime).HasColumnName("start_time").IsRequired();
            b.Property(s => s.EndTime).HasColumnName("end_time").IsRequired();
        });
    }
}
