using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration;

public class ServiceCancellationRequestConfiguration : IEntityTypeConfiguration<ServiceCancellationRequest>
{
    public void Configure(EntityTypeBuilder<ServiceCancellationRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => CancellationRequestId.From(value))
            .IsRequired();

        builder.Property(x => x.ExecutionId)
            .HasConversion(id => id.Value, value => ServiceExecutionId.From(value))
            .IsRequired();

        builder.Property(x => x.CancelledBy)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Reason).IsRequired();
        builder.Property(x => x.Notes);
        builder.Property(x => x.RequestReassignment).IsRequired();
        builder.Property(x => x.RequestedAt).IsRequired();
    }
}