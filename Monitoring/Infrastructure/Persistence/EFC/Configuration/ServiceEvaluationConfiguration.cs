using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration;

public class ServiceEvaluationConfiguration : IEntityTypeConfiguration<ServiceEvaluation>
{
    public void Configure(EntityTypeBuilder<ServiceEvaluation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => EvaluationId.From(value))
            .IsRequired();

        builder.Property(x => x.ExecutionId)
            .HasConversion(id => id.Value, value => ServiceExecutionId.From(value))
            .IsRequired();

        builder.Property(x => x.ReviewerId).IsRequired();
        builder.Property(x => x.ReviewedId).IsRequired();
        builder.Property(x => x.ReviewerRole).IsRequired();
        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.Comment);
        builder.Property(x => x.CategoriesJson).IsRequired();
        builder.Property(x => x.SubmittedAt).IsRequired();

    }
}

