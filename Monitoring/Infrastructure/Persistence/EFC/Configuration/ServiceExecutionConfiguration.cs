using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration;

public class ServiceExecutionConfiguration : IEntityTypeConfiguration<ServiceExecution>
{
    public void Configure(EntityTypeBuilder<ServiceExecution> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => ServiceExecutionId.From(value))
            .IsRequired();

        builder.Property(x => x.AssignmentId)
            .HasConversion(id => id.Value, value => AssignmentId.From(value))
            .IsRequired();

        builder.Property(x => x.RequestId)
            .HasConversion(id => id.Value, value => RequestId.From(value))
            .IsRequired();

        builder.Property(x => x.TechnicianId)
            .HasConversion(id => id.Value, value => TechnicianId.From(value))
            .IsRequired();

        builder.Property(x => x.HomeownerId)
            .HasConversion(id => id.Value, value => HomeownerId.From(value))
            .IsRequired();

        builder.Property(x => x.PropertyId)
            .HasConversion(id => id.Value, value => PropertyId.From(value))
            .IsRequired();
        
        builder.Property(x => x.HomeownerEvaluationId)
            .HasConversion(id => id!.Value, value => EvaluationId.From(value))
            .IsRequired(false);

        builder.Property(x => x.TechnicianEvaluationId)
            .HasConversion(id => id!.Value, value => EvaluationId.From(value))
            .IsRequired(false);
        
        builder.Property(x => x.CancellationRequestId)
            .HasConversion(id => id!.Value, value => CancellationRequestId.From(value))
            .IsRequired(false);
        
        builder.Property(x => x.RecipeSnapshot)
            .HasConversion(
                recipe => JsonSerializer.Serialize(recipe, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<RecipeSnapshot>(json, (JsonSerializerOptions?)null)!
            )
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.ScheduledDateTime).IsRequired();
        builder.Property(x => x.StartedAt);
        builder.Property(x => x.CompletedAt);
        builder.Property(x => x.CancelledAt);

        builder.OwnsMany(x => x.WorkPhotos, wp =>
        {
            wp.WithOwner().HasForeignKey("ExecutionId");
            wp.HasKey(x => x.Id);
            wp.Property(x => x.Id).HasConversion(id => id.Value, v => WorkPhotoId.From(v)).IsRequired();
            wp.Property(p => p.ExecutionId).HasConversion(id => id.Value, v => ServiceExecutionId.From(v)).IsRequired();
            wp.Property(p => p.PhotoType).HasConversion<string>().IsRequired();
            wp.Property(p => p.PhotoUrl).IsRequired();
            wp.Property(p => p.TakenAt).IsRequired();
            wp.Property(p => p.Notes);
        });

        builder.OwnsMany(x => x.ComponentSubstitutions, cs =>
        {
            cs.WithOwner().HasForeignKey("ExecutionId");
            cs.HasKey(x => x.Id);
            cs.Property(x => x.Id).HasConversion(id => id.Value, v => ComponentUsageId.From(v)).IsRequired();
            cs.Property(c => c.ExecutionId).HasConversion(id => id.Value, v => ServiceExecutionId.From(v)).IsRequired();
            cs.Property(c => c.ComponentTypeId).IsRequired();
            cs.Property(c => c.ComponentTypeName).IsRequired();
            cs.Property(c => c.QuantityUsed).IsRequired();
            cs.Property(c => c.QuantityReserved).IsRequired();
            cs.Ignore(c => c.Delta);
        });

        builder.OwnsMany(x => x.Evaluations, ev =>
        {
            ev.ToTable("ServiceEvaluations");
            ev.WithOwner().HasForeignKey("ExecutionId");
            ev.HasKey(x => x.Id);
            ev.Property(x => x.Id)
                .HasConversion(id => id.Value, v => EvaluationId.From(v))
                .IsRequired();
            ev.Property(e => e.ExecutionId)
                .HasConversion(id => id.Value, v => ServiceExecutionId.From(v))
                .IsRequired();
            ev.Property(e => e.ReviewerId).IsRequired();
            ev.Property(e => e.ReviewedId).IsRequired();
            ev.Property(e => e.ReviewerRole).IsRequired();
            ev.Property(e => e.Rating).IsRequired();
            ev.Property(e => e.Comment);
            ev.Property(e => e.CategoriesJson).IsRequired();
            ev.Property(e => e.SubmittedAt).IsRequired();
        });
    }
}
