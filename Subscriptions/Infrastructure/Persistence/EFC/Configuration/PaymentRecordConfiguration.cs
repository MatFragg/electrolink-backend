using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration;

public class PaymentRecordConfiguration : IEntityTypeConfiguration<PaymentRecord>
{
    public void Configure(EntityTypeBuilder<PaymentRecord> builder)
    {
        builder.ToTable("sp_payment_records");

        builder.HasKey(p => p.PaymentRecordId);

        builder.Property(p => p.PaymentRecordId)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => PaymentRecordId.From(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.SubscriptionId)
            .HasColumnName("subscription_id")
            .HasConversion(id => id.Value, value => SubscriptionId.From(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.StripeInvoiceId)
            .HasColumnName("stripe_invoice_id")
            .HasConversion(id => id.Value, value => StripeInvoiceId.From(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.StripePaymentIntentId)
            .HasColumnName("stripe_payment_intent_id")
            .HasConversion(id => id!.Value, value => StripePaymentIntentId.From(value))
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion(s => s.ToString(), v => PaymentStatus.From(v))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.ProcessedAt)
            .HasColumnName("processed_at")
            .IsRequired();

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.WithOwner().HasForeignKey("id");
            money.Property(m => m.Amount)
                .HasColumnName("amount")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.HasIndex(p => p.StripeInvoiceId)
            .IsUnique()
            .HasDatabaseName("uix_sp_payment_records_invoice");

        builder.HasIndex(p => p.SubscriptionId)
            .HasDatabaseName("ix_sp_payment_records_subscription");
    }
}

