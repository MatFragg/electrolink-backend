using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    // ── Conversores reutilizables ──────────────────────────────────────────
    private static readonly ValueConverter<ProfileId, string> ProfileIdConverter =
        new(id => id.Value, raw => ProfileId.From(raw));

    private static readonly ValueConverter<UserId, string> UserIdConverter =
        new(id => id.Value, raw => UserId.From(raw));

    private static readonly ValueConverter<HomeownerId, string> HomeownerIdConverter =
        new(id => id.Value, raw => HomeownerId.From(raw));

    private static readonly ValueConverter<TechnicianId, string> TechnicianIdConverter =
        new(id => id.Value, raw => TechnicianId.From(raw));

    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Profile>(b =>
        {
            b.HasKey(p => p.ProfileId);
            b.Property(p => p.ProfileId)
                .HasConversion(ProfileIdConverter)
                .HasColumnName("profile_id")   // <-- nombre explícito en la PK
                .IsRequired();

            b.Property(p => p.UserId)
                .HasConversion(UserIdConverter)
                .HasColumnName("user_id")
                .IsRequired();

            b.Property(p => p.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .IsRequired();

            b.Property(p => p.BusinessRole)
                .HasConversion<string>()
                .HasColumnName("business_role");

            b.OwnsOne(p => p.PersonalData, n =>
            {
                n.WithOwner().HasForeignKey("profile_id"); 

                n.Property(pd => pd.FirstName).HasColumnName("first_name").IsRequired();
                n.Property(pd => pd.LastName).HasColumnName("last_name").IsRequired();

                n.OwnsOne(pd => pd.PhoneNumber, pn =>
                {
                    pn.WithOwner().HasForeignKey("profile_id");
                    pn.Property(v => v.Value)
                        .HasColumnName("phone_number")
                        .IsRequired();
                });

                n.OwnsOne(pd => pd.Dni, d =>
                {
                    d.WithOwner().HasForeignKey("profile_id");
                    d.Property(v => v.Value)
                        .HasColumnName("dni")
                        .IsRequired();
                });

                n.OwnsOne(pd => pd.DateOfBirth, db =>
                {
                    db.WithOwner().HasForeignKey("profile_id");
                    db.Property(v => v.Value)
                        .HasColumnName("date_of_birth")
                        .IsRequired();
                });

                n.OwnsOne(pd => pd.Address, a =>
                {
                    a.WithOwner().HasForeignKey("profile_id");
                    a.Property(s => s.Street);
                    a.Property(s => s.Number);
                    a.Property(s => s.District);
                    a.Property(s => s.City);
                    a.Property(s => s.PostalCode);
                    a.Property(s => s.Country);
                });
            });

            b.HasOne(p => p.Homeowner)
                .WithOne()
                .HasForeignKey<HomeOwner>(ho => ho.ProfileId);

            b.HasOne(p => p.Technician)
                .WithOne()
                .HasForeignKey<Technician>(t => t.ProfileId);
        });

        // ── HomeOwner ─────────────────────────────────────────────────────
        builder.Entity<HomeOwner>(b =>
        {
            b.HasKey(ho => ho.HomeownerId);
            b.Property(ho => ho.HomeownerId)
             .HasConversion(HomeownerIdConverter)
             .IsRequired();

            b.Property(ho => ho.ProfileId)
             .HasConversion(ProfileIdConverter)
             .IsRequired();

            b.Property(ho => ho.PreferredContactTime)
             .HasConversion<string>()
             .IsRequired();

            b.OwnsOne(ho => ho.CommunicationPreferences, cp =>
            {
                cp.Property(c => c.SmsNotifications);
                cp.Property(c => c.EmailNotifications);
                cp.Property(c => c.PushNotifications);
                cp.Property(c => c.PreferredContactTime).HasConversion<string>();
            });

            b.OwnsOne(ho => ho.EmergencyContact);
        });

        // ── Technician ────────────────────────────────────────────────────
        builder.Entity<Technician>(b =>
        {
            b.HasKey(t => t.TechnicianId);
            b.Property(t => t.TechnicianId)
             .HasConversion(TechnicianIdConverter)
             .IsRequired();

            b.Property(t => t.ProfileId)
             .HasConversion(ProfileIdConverter)
             .IsRequired();

            b.Property(t => t.ExperienceYears).IsRequired();
            b.Property(t => t.AboutMe).HasMaxLength(2000);
            
            b.OwnsOne(t => t.ServiceArea, sa =>
            {
                sa.Property(v => v.CenterLatitude)
                    .HasColumnName("service_area_lat")
                    .HasColumnType("decimal(9,6)")
                    .IsRequired();

                sa.Property(v => v.CenterLongitude)
                    .HasColumnName("service_area_lon")
                    .HasColumnType("decimal(9,6)")
                    .IsRequired();

                sa.Property(v => v.RadiusKm)
                    .HasColumnName("service_area_radius_km")
                    .HasColumnType("decimal(6,2)")
                    .IsRequired();

                sa.Property(v => v.Area)
                    .HasColumnName("service_area_geom")
                    .HasColumnType("geometry(Polygon, 4326)")
                    .IsRequired();
                
                sa.HasIndex(v => v.Area)
                    .HasMethod("GIST");
            });
            
            b.HasMany(typeof(TechnicianSpecialty), "_specialtyEntities")
             .WithOne(nameof(TechnicianSpecialty.Technician))
             .HasForeignKey("TechnicianId")
             .IsRequired();
        });

        // ── TechnicianSpecialty ───────────────────────────────────────────
        builder.Entity<TechnicianSpecialty>(b =>
        {
            b.HasKey(nameof(TechnicianSpecialty.TechnicianId), nameof(TechnicianSpecialty.Specialty));

            b.Property(ts => ts.TechnicianId)
             .HasConversion(TechnicianIdConverter)
             .IsRequired();

            b.Property(ts => ts.Specialty)
             .HasConversion<int>()
             .IsRequired();

            b.HasOne(ts => ts.Technician)
             .WithMany("_specialtyEntities")
             .HasForeignKey(ts => ts.TechnicianId);
        });
    }
}