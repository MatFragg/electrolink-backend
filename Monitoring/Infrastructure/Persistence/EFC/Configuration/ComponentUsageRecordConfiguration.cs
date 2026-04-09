using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration;

public class ComponentUsageRecordConfiguration : IEntityTypeConfiguration<ComponentUsageRecord>
{
    public void Configure(EntityTypeBuilder<ComponentUsageRecord> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => ComponentUsageId.From(value))
            .IsRequired();

        builder.Property(x => x.ExecutionId)
            .HasConversion(id => id.Value, value => ServiceExecutionId.From(value))
            .IsRequired();

        builder.Property(x => x.ComponentTypeId).IsRequired();
        builder.Property(x => x.ComponentTypeName).IsRequired();
        builder.Property(x => x.QuantityUsed).IsRequired();
        builder.Property(x => x.QuantityReserved).IsRequired();

        builder.Ignore(x => x.Delta);
    }
}

