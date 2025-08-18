using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {

        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().OwnsOne(p => p.Name,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.FirstName).HasColumnName("FirstName");
                n.Property(p => p.LastName).HasColumnName("LastName");
            });
        
        builder.Entity<Profile>().OwnsOne(p => p.Email,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.Address).HasColumnName("EmailAddress");
            });
        
        builder.Entity<Profile>().OwnsOne(p => p.Address,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(s => s.Street).HasColumnName("AddressStreet");
                a.Property(s => s.Number).HasColumnName("AddressNumber");
                a.Property(s => s.City).HasColumnName("AddressCity");
                a.Property(s => s.PostalCode).HasColumnName("AddressPostalCode");
                a.Property(s => s.Country).HasColumnName("AddressCountry"); 
            });
        
        builder.Entity<HomeOwner>().HasKey(ho => ho.Id);
        builder.Entity<HomeOwner>().Property(ho => ho.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<HomeOwner>().Property(ho => ho.Dni).IsRequired().HasMaxLength(20);

        builder.Entity<Technician>().HasKey(t => t.Id);
        builder.Entity<Technician>().Property(t => t.Id).IsRequired().ValueGeneratedNever();
        builder.Entity<Technician>().Property(t => t.ProfileId).IsRequired();
        builder.Entity<Technician>().Property(t => t.LicenseNumber)
            .IsRequired().HasMaxLength(50);
        builder.Entity<Technician>().Property(t => t.Specialization)
            .IsRequired().HasMaxLength(100);
        builder.Entity<Technician>()
            .HasMany(t => t.PortfolioItems)
            .WithOne()
            .HasForeignKey(pi => pi.TechnicianId)
            .IsRequired();

        builder.Entity<PortfolioItem>().HasKey(pi => pi.Id);
        builder.Entity<PortfolioItem>().Property(pi => pi.Id).IsRequired().ValueGeneratedNever();
        builder.Entity<PortfolioItem>().Property(pi => pi.WorkId).IsRequired();
        builder.Entity<PortfolioItem>().HasIndex(pi => pi.WorkId).IsUnique();
        builder.Entity<PortfolioItem>().Property(pi => pi.Title).IsRequired().HasMaxLength(100);
        builder.Entity<PortfolioItem>().Property(pi => pi.Description).HasMaxLength(1000);
        builder.Entity<PortfolioItem>().Property(pi => pi.ImageUrl).HasMaxLength(500);
        builder.Entity<PortfolioItem>().Property(pi => pi.TechnicianId).IsRequired();
    }
}