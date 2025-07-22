using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAssetsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ComponentType>(b =>
        {
            b.HasKey(ct => ct.Id);
            b.Property(ct => ct.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(ct => ct.Name).IsRequired().HasMaxLength(50);
            b.Property(ct => ct.Description).HasMaxLength(250);
            b.Property(ct => ct.Id).HasConversion(id => id.Id, value => new ComponentTypeId(value));
        });

        builder.Entity<Component>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Id).HasConversion(id => id.Id, value => new ComponentId(value));
            b.Property(c => c.Name).IsRequired().HasMaxLength(100);
            b.Property(c => c.Description).HasMaxLength(500);
            b.Property(c => c.IsActive).IsRequired();
            b.Property(c => c.TypeId).HasConversion(id => id.Id, value => new ComponentTypeId(value)).HasColumnName("component_type_id");
            b.HasOne<ComponentType>().WithMany().HasForeignKey(c => c.TypeId).IsRequired();
        });

        builder.Entity<Property>(prop =>
        {
            prop.HasKey(p => p.Id);
            prop.Property(p => p.Id)
                .HasConversion(
                    id => id.Id,              
                    guid => new PropertyId(guid) 
                )
                .HasColumnName("property_id")
                .IsRequired()
                .ValueGeneratedNever(); 

            prop.Property(p => p.OwnerId)
                .HasConversion(
                    id => id.Id,              
                    guid => new OwnerId(guid)  
                )
                .HasColumnName("owner_id")
                .IsRequired();

            prop.OwnsOne(p => p.Address, addr =>
            {
                addr.Property(a => a.Street).IsRequired().HasMaxLength(120).HasColumnName("street");
                addr.Property(a => a.Number).IsRequired().HasMaxLength(20).HasColumnName("number");
            });

            prop.OwnsOne(p => p.Region, reg =>
            {
                reg.Property(r => r.Name).IsRequired().HasMaxLength(60).HasColumnName("region");
            });

            prop.OwnsOne(p => p.District, dist =>
            {
                dist.Property(d => d.Name).IsRequired().HasMaxLength(60).HasColumnName("district");
            });
        });

        builder.Entity<TechnicianInventory>(b =>
        {
            b.HasKey(i => i.Id);
            b.Property(i => i.Id).IsRequired().ValueGeneratedNever();
    
            b.Property(i => i.TechnicianId).HasConversion(id => id.Id, value => new TechnicianId(value)).IsRequired();
            b.HasIndex(i => i.TechnicianId).IsUnique();
    
            b.HasMany(i => i.StockItems)
                .WithOne(s => s.TechnicianInventory)
                .HasForeignKey(cs => cs.TechnicianInventoryId)
                .IsRequired();
        });

        builder.Entity<ComponentStock>(b =>
        {
            b.HasKey(cs => cs.Id);

            b.Property(cs => cs.ComponentId).HasConversion(id => id.Id, value => new ComponentId(value)).HasColumnName("component_id").IsRequired();

            b.HasIndex(cs => new { cs.TechnicianInventoryId, cs.ComponentId }).IsUnique();
    
            b.HasOne<Component>()
                .WithMany()
                .HasForeignKey(cs => cs.ComponentId)
                .IsRequired();

            b.Property(cs => cs.TechnicianInventoryId).IsRequired();
            b.Property(cs => cs.QuantityAvailable).IsRequired();
            b.Property(cs => cs.AlertThreshold).IsRequired();
            b.Property(cs => cs.LastUpdated).IsRequired();
        }); 
    }
}