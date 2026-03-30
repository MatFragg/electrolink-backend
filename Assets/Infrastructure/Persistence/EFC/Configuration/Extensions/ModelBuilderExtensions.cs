using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAssetsConfiguration(this ModelBuilder builder)
    {
        /*
         * Technician Inventory
         */
        builder.Entity<TechnicianInventory>().HasKey(ti => ti.Id);
        builder.Entity<TechnicianInventory>()
            .Property(ti => ti.Id)
            .HasConversion(id => id.Value, value => TechnicianInventoryId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<TechnicianInventory>()
            .Property(ti => ti.TechnicianId)
            .HasConversion(id => id.Value, value => TechnicianId.From(value))
            .HasColumnName("TechnicianId")
            .IsRequired();

        builder.Entity<TechnicianInventory>()
            .Property(ti => ti.Status)
            .HasConversion<string>()
            .HasColumnName("Status")
            .IsRequired();

        builder.Entity<TechnicianInventory>()
            .HasMany(ti => ti.StockItems)
            .WithOne()
            .HasForeignKey("TechnicianInventoryId")
            .IsRequired();

        builder.Entity<TechnicianInventory>()
            .HasMany(ti => ti.Reservations)
            .WithOne()
            .HasForeignKey("TechnicianInventoryId")
            .IsRequired();

        /*
         * Component Stock
         */
        builder.Entity<ComponentStock>().HasKey(cs => cs.Id);
        builder.Entity<ComponentStock>()
            .Property(cs => cs.Id)
            .HasConversion(id => id.Value, value => ComponentStockId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<ComponentStock>()
            .Property(cs => cs.TechnicianInventoryId)
            .HasConversion(id => id.Value, value => TechnicianInventoryId.From(value))
            .HasColumnName("TechnicianInventoryId")
            .IsRequired();

        builder.Entity<ComponentStock>()
            .Property(cs => cs.ComponentId)
            .HasConversion(id => id.Value, value => ComponentId.From(value))
            .HasColumnName("ComponentId")
            .IsRequired();
        
        builder.Entity<ComponentStock>()
            .Property(cs => cs.ComponentTypeId)
            .HasConversion(id => id.Value, value => ComponentTypeId.From(value))
            .HasColumnName("ComponentTypeId")
            .IsRequired();

        /*
         * Component Reservation
         */
        builder.Entity<ComponentReservation>().HasKey(cr => cr.Id);
        builder.Entity<ComponentReservation>()
            .Property(cr => cr.Id)
            .HasConversion(id => id.Value, value => ComponentReservationId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<ComponentReservation>()
            .Property(cr => cr.TechnicianInventoryId)
            .HasConversion(id => id.Value, value => TechnicianInventoryId.From(value))
            .HasColumnName("TechnicianInventoryId")
            .IsRequired();

        builder.Entity<ComponentReservation>()
            .Property(cr => cr.AssignmentId)
            .HasConversion(id => id.Value, value => AssignmentId.From(value))
            .HasColumnName("ServiceId")
            .IsRequired();

        builder.Entity<ComponentReservation>()
            .HasMany(cr => cr.Items)
            .WithOne()
            .HasForeignKey("ReservationId")
            .IsRequired();

        /*
         * Reservation Item
         */
        builder.Entity<ReservationItem>().HasKey(ri => ri.Id);
        builder.Entity<ReservationItem>()
            .Property(ri => ri.Id)
            .HasConversion(id => id.Value, value => ReservationItemId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<ReservationItem>()
            .Property(ri => ri.ReservationId)
            .HasConversion(id => id.Value, value => ComponentReservationId.From(value))
            .HasColumnName("ReservationId")
            .IsRequired();

        builder.Entity<ReservationItem>()
            .Property(ri => ri.ComponentId)
            .HasConversion(id => id.Value, value => ComponentId.From(value))
            .HasColumnName("ComponentId")
            .IsRequired();
        
        builder.Entity<ReservationItem>()
            .Property(cs => cs.ComponentTypeId)
            .HasConversion(id => id.Value, value => ComponentTypeId.From(value))
            .HasColumnName("ComponentTypeId")
            .IsRequired();
        
        /*
         * Component Type
         */
        builder.Entity<ComponentType>().HasKey(ct => ct.Id);
        builder.Entity<ComponentType>()
            .Property(ct => ct.Id)
            .HasConversion(id => id.Value, value => ComponentTypeId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<ComponentType>().Property(ct => ct.Name).IsRequired();

        /*
         * Component
         */
        builder.Entity<Component>().HasKey(c => c.Id);
        builder.Entity<Component>()
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => ComponentId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<Component>().HasIndex(c => c.Name);
        builder.Entity<Component>().HasIndex(c => c.IsActive);

        /*
         * Property Portfolio
         */
        builder.Entity<PropertyPortfolio>().HasKey(pp => pp.Id);
        builder.Entity<PropertyPortfolio>()
            .Property(pp => pp.Id)
            .HasConversion(id => id.Value, value => PropertyPortfolioId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<PropertyPortfolio>()
            .Property(pp => pp.HomeownerId)
            .HasConversion(id => id.Value, value => HomeownerId.From(value))
            .HasColumnName("HomeownerId")
            .IsRequired();

        builder.Entity<PropertyPortfolio>()
            .Property(pp => pp.Status)
            .HasConversion<string>()
            .HasColumnName("Status")
            .IsRequired();

        builder.Entity<PropertyPortfolio>()
            .HasMany(pp => pp.Entries)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();

        /*
         * Portfolio Entry
         */
        builder.Entity<PortfolioEntry>().HasKey(pe => pe.Id);
        builder.Entity<PortfolioEntry>()
            .Property(pe => pe.Id)
            .HasConversion(id => id.Value, value => PortfolioEntryId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<PortfolioEntry>()
            .Property(pe => pe.PortfolioId)
            .HasConversion(id => id.Value, value => PropertyPortfolioId.From(value))
            .HasColumnName("PortfolioId")
            .IsRequired();

        builder.Entity<PortfolioEntry>()
            .Property(pe => pe.PropertyId)
            .HasConversion(id => id.Value, value => PropertyId.From(value))
            .HasColumnName("PropertyId")
            .IsRequired();

        builder.Entity<PortfolioEntry>()
            .Property(pe => pe.OccupancyStatus)
            .HasConversion<string>()
            .IsRequired();

        /*
         * Property
         */
        builder.Entity<Property>().HasKey(prop => prop.Id);
        builder.Entity<Property>()
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PropertyId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Entity<Property>()
            .Property(p => p.OwnerId)
            .HasConversion(id => id.Value, value => HomeownerId.From(value))
            .HasColumnName("OwnerId")
            .IsRequired();

        builder.Entity<Property>().HasIndex(p => p.OwnerId);

        builder.Entity<Property>().OwnsOne(p => p.Address, addr =>
        {
            addr.WithOwner().HasForeignKey("Id");
    
            addr.Property(s => s.Street).HasColumnName("Street");
            addr.Property(s => s.Number).HasColumnName("Number");
            addr.Property(s => s.District).HasColumnName("District");
            addr.Property(s => s.City).HasColumnName("City");
            addr.Property(s => s.PostalCode).HasColumnName("PostalCode");
            addr.Property(s => s.Country).HasColumnName("Country");
        });

        builder.Entity<Property>().OwnsOne(p => p.Geolocation, geo =>
        {
            geo.WithOwner().HasForeignKey("Id");
    
            geo.Property(g => g.Latitude).HasColumnName("Latitude");
            geo.Property(g => g.Longitude).HasColumnName("Longitude");
            geo.Property(g => g.Accuracy).HasColumnName("Accuracy");
            geo.Property(g => g.Source).HasColumnName("Source");
        });

        builder.Entity<Property>()
            .Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Entity<Property>()
            .Property(p => p.IsActive)
            .IsRequired();
    }
}
