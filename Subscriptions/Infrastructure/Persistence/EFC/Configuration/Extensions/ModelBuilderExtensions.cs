using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
/// Extension methods for <see cref="ModelBuilder"/> to apply Subscription and Payments configurations.
/// </summary>
public static class ModelBuilderExtensions
{
    
    /// <summary>
    /// Applies the Entity Framework Core configuration for the Subscription and Payments Bounded Context.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance.</param>
    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)
    {
        // Subscription Aggregate Configuration
        builder.Entity<Subscription>(subscriptionConfiguration =>
        {
            // Define primary key as the Value of the SubscriptionId Value Object
            subscriptionConfiguration.HasKey(s => s.Id);
            
            // Configure the Id property conversion (remove OwnsOne for Id)
            subscriptionConfiguration.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    value => new SubscriptionId(value))
                .HasColumnName("SubscriptionId")
                .ValueGeneratedOnAdd();

            // Configure other Value Object properties (keep these)
            subscriptionConfiguration.Property(s => s.UserId)
                .HasConversion(
                    userId => userId.Value, // Convierte UserId a int para la DB
                    value => new UserId(value)) // Convierte int a UserId para la aplicación
                .HasColumnName("UserId") // Nombre de la columna en la tabla
                .IsRequired();
            subscriptionConfiguration.Property(s => s.PlanId)
                .HasConversion(
                    planId => planId.Value,
                    value => new PlanId(value))
                .HasColumnName("PlanId")
                .IsRequired();
            // Configure other properties...
            subscriptionConfiguration.Property(s => s.Status)
                .IsRequired()
                .HasConversion<string>();
            subscriptionConfiguration.Property(s => s.StartDate).IsRequired();
            subscriptionConfiguration.Property(s => s.EndDate).IsRequired();
            subscriptionConfiguration.Property(s => s.CancellationEffectiveDate);
            subscriptionConfiguration.Property(s => s.TrialEndsAt);
            subscriptionConfiguration.Property(s => s.GatewayCustomerId)
                .HasConversion(
                    pgId => pgId.Value,
                    value => new PaymentGatewayCustomerId(value))
                .IsRequired()
                .HasMaxLength(100);
            subscriptionConfiguration.Property(s => s.GatewaySubscriptionId)
                .HasConversion(
                    pgId => pgId.Value,
                    value => new PaymentGatewaySubscriptionId(value))
                .IsRequired()
                .HasMaxLength(100);
            subscriptionConfiguration.Property(s => s.CurrentUsage).IsRequired().HasDefaultValue(0);
        });

        // Aplicar la misma corrección para Plan
        builder.Entity<Plan>(planConfiguration =>
        {
            planConfiguration.HasKey(p => p.Id);
            
            // Configure the Id property conversion (remove OwnsOne for Id)
            planConfiguration.Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    value => new PlanId(value))
                .HasColumnName("PlanId")
                .ValueGeneratedOnAdd();

            // Resto de la configuración sin cambios...
            planConfiguration.Property(p => p.Name).IsRequired().HasMaxLength(100);
            planConfiguration.Property(p => p.Description).HasMaxLength(500);
            planConfiguration.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            planConfiguration.Property(p => p.Currency).IsRequired().HasMaxLength(3);
            planConfiguration.Property(p => p.MonetizationType)
                .IsRequired()
                .HasConversion<string>();
            planConfiguration.Property(p => p.IsDefault).IsRequired();
            planConfiguration.Property(p => p.TargetRole)
                .IsRequired()
                .HasConversion<string>();
            planConfiguration.Property(p => p.GatewayPriceId)
                .HasConversion(
                    pgId => pgId == null ? null : pgId.Value,
                    value => value == null ? null : new PaymentGatewayPriceId(value))
                .HasMaxLength(100);

            planConfiguration.OwnsMany(p => p.Benefits, benefitBuilder =>
            {
                benefitBuilder.ToJson();
                benefitBuilder.Property(b => b.Type).IsRequired();
                benefitBuilder.Property(b => b.Description);
                benefitBuilder.Property(b => b.LimitValue);
                benefitBuilder.Property(b => b.FlagValue);
            });
        });

        // PaymentTransaction configuration remains the same...
        builder.Entity<PaymentTransaction>(paymentTransactionConfiguration =>
        {
            paymentTransactionConfiguration.HasKey(pt => pt.Id);
            paymentTransactionConfiguration.Property(pt => pt.Id)
                .HasColumnName("Id") 
                .ValueGeneratedOnAdd();

            paymentTransactionConfiguration.Property(pt => pt.SubscriptionId)
                .HasConversion(
                    id => id.Value,
                    value => new SubscriptionId(value))
                .HasColumnName("SubscriptionId")
                .IsRequired();

            paymentTransactionConfiguration.Property(pt => pt.Amount).IsRequired().HasColumnType("decimal(18,2)");
            paymentTransactionConfiguration.Property(pt => pt.Currency).IsRequired().HasMaxLength(3);
            paymentTransactionConfiguration.Property(pt => pt.TransactionDate).IsRequired();
            paymentTransactionConfiguration.Property(pt => pt.Status)
                .IsRequired()
                .HasConversion<string>();
            paymentTransactionConfiguration.Property(pt => pt.GatewayTransactionId).IsRequired().HasMaxLength(255);
            paymentTransactionConfiguration.Property(pt => pt.Message).HasMaxLength(500);
        });
        
        builder.ApplyConfiguration(new WebhookEventConfiguration()); 
    }
}