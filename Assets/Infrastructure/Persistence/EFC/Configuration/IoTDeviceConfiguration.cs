using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration;

public class IoTDeviceConfiguration : IEntityTypeConfiguration<IoTDevice>
{
    public void Configure(EntityTypeBuilder<IoTDevice> builder)
    {
        builder.ToTable("arm_iot_devices");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, v => IoTDeviceId.From(v))
            .HasColumnName("id")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(d => d.SerialNumber)
            .HasConversion(s => s.Value, v => SerialNumber.From(v))
            .HasColumnName("serial_number")
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(d => d.SerialNumber).IsUnique();

        builder.Property(d => d.ApiKeyHash)
            .HasConversion(h => h.Value, v => ApiKeyHash.From(v))
            .HasColumnName("api_key_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.FirmwareVersion)
            .HasColumnName("firmware_version")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(d => d.AssignedPropertyId)
            .HasConversion(
                id => id == null ? null : id.Value,
                v => v == null ? null : PropertyId.From(v))
            .HasColumnName("assigned_property_id")
            .HasMaxLength(60);

        builder.Property(d => d.InstallationRequestId)
            .HasConversion(
                id => id == null ? null : id.Value,
                v => v == null ? null : InstallationRequestId.From(v))
            .HasColumnName("installation_request_id")
            .HasMaxLength(60);

        builder.Property(d => d.InstalledByTechnicianId)
            .HasConversion(
                id => id == null ? null : id.Value,
                v => v == null ? null : TechnicianId.From(v))
            .HasColumnName("installed_by_technician_id")
            .HasMaxLength(60);

        builder.Property(d => d.InstalledAt)
            .HasColumnName("installed_at");

        builder.Property(d => d.ConnectionStatus)
            .HasConversion<string>()
            .HasColumnName("connection_status")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.LastReadingAt)
            .HasColumnName("last_reading_at");

        builder.Property(d => d.MaintenanceReason)
            .HasColumnName("maintenance_reason")
            .HasMaxLength(500);

        builder.Property(d => d.ExpectedReturnDate)
            .HasColumnName("expected_return_date");

        builder.HasIndex(d => d.Status).HasDatabaseName("idx_arm_iot_devices_status");
        builder.HasIndex(d => d.AssignedPropertyId).HasDatabaseName("idx_arm_iot_devices_property");
    }
}
