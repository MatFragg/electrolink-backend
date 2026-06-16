using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Persistence.EFC.Configurations;

public class RelayControlCommandConfiguration : IEntityTypeConfiguration<RelayControlCommand>
{
    public void Configure(EntityTypeBuilder<RelayControlCommand> builder)
    {
        builder.ToTable("iot_relay_control_commands");
        builder.HasKey(r => r.CommandId);

        builder.Property(r => r.CommandId)
            .HasColumnName("command_id").HasMaxLength(60)
            .HasConversion(id => id.Value, v => RelayCommandId.From(v))
            .IsRequired().ValueGeneratedNever();

        builder.Property(r => r.DeviceId)
            .HasColumnName("device_id").HasMaxLength(60)
            .HasConversion(id => id.Value, v => DeviceId.From(v))
            .IsRequired();

        builder.Property(r => r.PropertyId)
            .HasColumnName("property_id").HasMaxLength(60)
            .HasConversion(id => id.Value, v => PropertyId.From(v))
            .IsRequired();

        builder.Property(r => r.TargetRelayState)
            .HasColumnName("target_relay_state").HasMaxLength(10)
            .HasConversion<string>().IsRequired();

        builder.Property(r => r.RequestedBy).HasColumnName("requested_by").HasMaxLength(60).IsRequired();
        builder.Property(r => r.AuthorizationSource).HasColumnName("authorization_source").HasMaxLength(40).IsRequired();

        builder.Property(r => r.ServiceRequestId)
            .HasColumnName("service_request_id").HasMaxLength(60)
            .HasConversion(
                id => id == null ? null : id.Value,
                v  => v == null  ? null : ServiceRequestId.From(v));

        builder.Property(r => r.Status)
            .HasColumnName("status").HasMaxLength(20)
            .HasConversion<string>().IsRequired();

        builder.Property(r => r.IssuedAt).HasColumnName("issued_at").IsRequired();
        builder.Property(r => r.SentAt).HasColumnName("sent_at");
        builder.Property(r => r.AcknowledgedAt).HasColumnName("acknowledged_at");
        builder.Property(r => r.ExecutedAt).HasColumnName("executed_at");
        builder.Property(r => r.FailureReason).HasColumnName("failure_reason").HasMaxLength(500);
        builder.Property(r => r.RetryCount).HasColumnName("retry_count").IsRequired();

        builder.Property(r => r.CreatedDate).HasColumnName("created_at").IsRequired();
        builder.Property(r => r.UpdatedDate).HasColumnName("updated_at");

        builder.HasIndex(r => r.DeviceId);
        builder.HasIndex(r => r.Status);

        builder.Ignore(r => r.DomainEvents);
    }
}
