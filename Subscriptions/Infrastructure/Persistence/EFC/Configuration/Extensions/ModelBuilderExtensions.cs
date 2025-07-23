using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;


public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Subscription>(ConfigureSubscription);
        builder.Entity<Plan>(ConfigurePlan);
    }

    private static void ConfigureSubscription(EntityTypeBuilder<Subscription> entity)
    {
        entity.ToTable("subscriptions");

        entity.HasKey(s => s.Id);
    
        // Cambia la configuración de la propiedad Id aquí
        entity.Property(s => s.Id)
            .HasColumnName("subscription_id") // <-- AÑADE ESTA LÍNEA
            .HasConversion(id => id.Value, value => new SubscriptionId(value))
            .ValueGeneratedNever();

        entity.Property(s => s.UserId)
            .HasConversion(id => id.Value, value => new UserId(value));

        entity.Property(s => s.PlanId)
            .HasConversion(id => id.Value, value => new PlanId(value));

        entity.Property(s => s.Status)
            .HasConversion<string>();

        entity.OwnsOne(s => s.PremiumAccess);
        entity.OwnsOne(s => s.Certification);
        entity.OwnsOne(s => s.Boost, boost =>
        {
            boost.Property(b => b.LastActivatedAt).HasColumnName("BoostLastActivatedAt");
            boost.WithOwner(); 
        });
    }

    private static void ConfigurePlan(EntityTypeBuilder<Plan> entity)
    {
        entity.ToTable("plans");

        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new PlanId(value))
            .ValueGeneratedNever();

        entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        entity.Property(p => p.Description).HasMaxLength(500);
        entity.Property(p => p.Price).HasPrecision(18, 2);
        entity.Property(p => p.Currency).HasMaxLength(10);
        entity.Property(p => p.MonetizationType)
            .HasConversion<string>();
        entity.Property(p => p.IsDefault);
    }
}