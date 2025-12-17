using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration;

public class WebhookEventConfiguration : IEntityTypeConfiguration<WebhookEvent>
{
    public void Configure(EntityTypeBuilder<WebhookEvent> builder)
    {
        builder.ToTable("webhook_events");

        // Primary Key
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id)
            .HasColumnName("id")
            .HasConversion(
                v => v.Value,
                v => new WebhookEventId(v))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(w => w.EventType)
            .HasColumnName("event_type")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(w => w.RawPayload)
            .HasColumnName("raw_payload")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(w => w.StripeCreatedAt)
            .HasColumnName("stripe_created_at")
            .IsRequired();

        builder.Property(w => w.ProcessedAt)
            .HasColumnName("processed_at");

        builder.Property(w => w.IsProcessed)
            .HasColumnName("is_processed")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(w => w.ErrorMessage)
            .HasColumnName("error_message")
            .HasMaxLength(1000);

        builder.Property(w => w.ProcessingAttempts)
            .HasColumnName("processing_attempts")
            .HasDefaultValue(0)
            .IsRequired();

        // Indices para mejorar performance
        builder.HasIndex(w => w.EventType)
            .HasDatabaseName("idx_webhook_events_event_type");

        builder.HasIndex(w => w.IsProcessed)
            .HasDatabaseName("idx_webhook_events_is_processed");

        builder.HasIndex(w => w.StripeCreatedAt)
            .HasDatabaseName("idx_webhook_events_stripe_created_at");
    }
}