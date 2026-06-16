using Hampcoders.Electrolink.API.Processing.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Persistence.EFC.Configurations;

public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
{
    public void Configure(EntityTypeBuilder<Reading> builder)
    {
        builder.ToTable("iot_readings");
        builder.HasKey(r => r.ReadingId);

        builder.Property(r => r.ReadingId)
            .HasColumnName("reading_id").HasMaxLength(100)
            .HasConversion(id => id.Value, v => ReadingId.From(v))
            .IsRequired().ValueGeneratedNever();

        builder.Property(r => r.StreamId)
            .HasColumnName("stream_id").HasMaxLength(60)
            .HasConversion(id => id.Value, v => StreamId.From(v))
            .IsRequired();

        builder.Property(r => r.Timestamp).HasColumnName("timestamp").IsRequired();
        builder.Property(r => r.Voltage).HasColumnName("voltage").IsRequired();
        builder.Property(r => r.Current).HasColumnName("current").IsRequired();
        builder.Property(r => r.PowerFactor).HasColumnName("power_factor").IsRequired();
        builder.Property(r => r.Frequency).HasColumnName("frequency").IsRequired();

        builder.Property(r => r.Source)
            .HasColumnName("source").HasMaxLength(30)
            .HasConversion<string>().IsRequired();

        builder.HasIndex(r => r.Timestamp);
    }
}
