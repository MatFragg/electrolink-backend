using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("sp_subscriptions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => new SubscriptionId(value));

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(s => s.PlanId)
            .HasColumnName("plan_id")
            .HasConversion(id => id.Value, value => new PlanId(value))
            .IsRequired();

        builder.Property(s => s.BusinessRole)
            .HasColumnName("business_role")
            .HasConversion(role => role.ToString(), value => BusinessRole.From(value))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.PlanType)
            .HasColumnName("plan_type")
            .HasConversion(plan => plan.ToString(), value => PlanType.From(value))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.BillingCycle)
            .HasColumnName("billing_cycle")
            .HasConversion(
                cycle => cycle == null ? null : cycle.ToString(),
                value => string.IsNullOrWhiteSpace(value) ? null : BillingCycle.From(value))
            .HasMaxLength(20);

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.CancelAtPeriodEnd)
            .HasColumnName("cancel_at_period_end")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(s => s.GatewayCustomerId)
            .HasColumnName("stripe_customer_id")
            .HasConversion(id => id.Value, value => new PaymentGatewayCustomerId(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.GatewaySubscriptionId)
            .HasColumnName("stripe_subscription_id")
            .HasConversion(
                id => id == null ? null : id.Value,
                value => string.IsNullOrWhiteSpace(value) ? null : new PaymentGatewaySubscriptionId(value))
            .HasMaxLength(100);

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(s => s.CancellationEffectiveDate)
            .HasColumnName("cancellation_effective_date");

        builder.Property(s => s.TrialEndsAt)
            .HasColumnName("trial_ends_at");

        builder.Property(s => s.GracePeriodEndsAt)
            .HasColumnName("grace_period_ends_at");

        builder.Property(s => s.CurrentUsage)
            .HasColumnName("current_usage")
            .HasDefaultValue(0)
            .IsRequired();

        builder.OwnsOne(s => s.BillingPeriod, period =>
        {
            period.Property(p => p.PeriodStart).HasColumnName("period_start");
            period.Property(p => p.PeriodEnd).HasColumnName("period_end");
        });

        builder.OwnsOne(s => s.UsageCounters, usage =>
        {
            usage.Property(u => u.MonthlyRequestsUsed)
                .HasColumnName("monthly_requests_used")
                .HasDefaultValue(0);
            usage.Property(u => u.MonthlyRequestsLimit)
                .HasColumnName("monthly_requests_limit")
                .HasDefaultValue(2);
        });

        builder.HasIndex(s => s.UserId)
            .IsUnique()
            .HasDatabaseName("uix_sp_subscriptions_user");

        builder.HasIndex(s => s.GatewayCustomerId)
            .HasDatabaseName("ix_sp_subscriptions_stripe_cust");

        builder.HasIndex(s => s.GatewaySubscriptionId)
            .HasDatabaseName("ix_sp_subscriptions_stripe_sub");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("ix_sp_subscriptions_status");

        builder.HasIndex(s => s.GracePeriodEndsAt)
            .HasDatabaseName("ix_sp_subscriptions_grace");
    }
}

