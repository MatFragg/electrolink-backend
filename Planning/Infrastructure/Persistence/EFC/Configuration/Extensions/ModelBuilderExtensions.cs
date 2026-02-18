using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyServiceDesignAndPlanningConfiguration(this ModelBuilder builder)
    {
        // --- ServiceCatalog Aggregate ---
        builder.Entity<ServiceCatalog>(b =>
        {
            b.ToTable("service_catalogs");

            b.HasKey(c => c.Id);
            b.Property(c => c.Id)
                .HasConversion(
                    v => v.Id,
                    v => new CatalogId(v))
                .HasColumnName("catalog_id")
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(c => c.TechnicianId)
                .HasConversion(
                    v => v.Id,
                    v => new TechnicianId(v))
                .HasColumnName("technician_id")
                .IsRequired();

            b.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            // ServiceRecipes collection (owned entities)
            b.OwnsMany(c => c.Recipes, recipe => {
                recipe.ToTable("service_recipes");

                // Ignorar colecciones CLR que se almacenan como JSON en campos de respaldo
                recipe.Ignore(r => r.ComponentRequirements);
                recipe.Ignore(r => r.Prerequisites);
                recipe.Ignore(r => r.Deliverables);

                // Usar la propiedad CLR existente como FK (nombre: "CatalogId")
                recipe.WithOwner().HasForeignKey("CatalogId");

                recipe.HasKey(nameof(ServiceRecipe.Id));

                recipe.Property(r => r.Id)
                    .HasConversion(
                        v => v.Id,
                        v => new RecipeId(v))
                    .HasColumnName("recipe_id")
                    .IsRequired()
                    .ValueGeneratedNever();

                // Mapear la propiedad CLR CatalogId (value object) a la columna Guid catalog_id
                recipe.Property(r => r.CatalogId)
                    .HasConversion(
                        v => v.Id,
                        v => new CatalogId(v))
                    .HasColumnName("catalog_id")
                    .IsRequired();

                // Backing JSON fields
                recipe.Property<string>("_componentRequirementsJson")
                    .HasColumnName("component_requirements_json")
                    .HasColumnType("jsonb");

                recipe.Property<string>("_prerequisitesJson")
                    .HasColumnName("prerequisites_json")
                    .HasColumnType("jsonb");

                recipe.Property<string>("_deliverablesJson")
                    .HasColumnName("deliverables_json")
                    .HasColumnType("jsonb");

                recipe.OwnsOne(r => r.ServiceName, sn =>
                {
                    // Forzar que el owned comparta la misma columna PK/FK que ServiceRecipe.recipe_id
                    sn.WithOwner().HasForeignKey("recipe_id");
                    sn.HasKey("recipe_id");

                    // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                    sn.Property<Guid>("recipe_id")
                        .HasColumnName("recipe_id")
                        .IsRequired();
                    
                    sn.Property(n => n.Value).HasColumnName("service_name").HasMaxLength(150).IsRequired();
                });

                recipe.Property(r => r.ServiceDescription)
                    .HasColumnName("service_description")
                    .HasMaxLength(500);

                recipe.Property(r => r.ServiceCategory)
                    .HasConversion<string>()
                    .HasColumnName("service_category")
                    .HasMaxLength(50)
                    .IsRequired();

                recipe.OwnsOne(r => r.Pricing, pricing =>
{
                    // Forzar que el owned Pricing use la misma columna PK/FK que ServiceRecipe.recipe_id
                    pricing.WithOwner().HasForeignKey("recipe_id");
                    pricing.HasKey("recipe_id");

                    // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                    pricing.Property<Guid>("recipe_id")
                        .HasColumnName("recipe_id")
                        .IsRequired();

                    // MaterialsEstimate (Money) - reutilizar recipe_id
                    pricing.OwnsOne(p => p.MaterialsEstimate, money =>
                    {
                        money.WithOwner().HasForeignKey("recipe_id");
                        money.HasKey("recipe_id");

                        money.Property<Guid>("recipe_id")
                            .HasColumnName("recipe_id")
                            .IsRequired();

                        money.Property(m => m.Amount)
                            .HasColumnName("materials_estimate_amount")
                            .HasPrecision(18, 2);
                        money.Property(m => m.Currency)
                            .HasColumnName("materials_estimate_currency")
                            .HasMaxLength(3);
                    });

                    // LaborCost (Money) - reutilizar recipe_id
                    pricing.OwnsOne(p => p.LaborCost, money =>
                    {
                        money.WithOwner().HasForeignKey("recipe_id");
                        money.HasKey("recipe_id");

                        money.Property<Guid>("recipe_id")
                            .HasColumnName("recipe_id")
                            .IsRequired();

                        money.Property(m => m.Amount)
                            .HasColumnName("labor_cost_amount")
                            .HasPrecision(18, 2);
                        money.Property(m => m.Currency)
                            .HasColumnName("labor_cost_currency")
                            .HasMaxLength(3);
                    });

                    // TotalPrice (Money) - reutilizar recipe_id
                    pricing.OwnsOne(p => p.TotalPrice, money =>
                    {
                        money.WithOwner().HasForeignKey("recipe_id");
                        money.HasKey("recipe_id");

                        money.Property<Guid>("recipe_id")
                            .HasColumnName("recipe_id")
                            .IsRequired();

                        money.Property(m => m.Amount)
                            .HasColumnName("total_price_amount")
                            .HasPrecision(18, 2);
                        money.Property(m => m.Currency)
                            .HasColumnName("total_price_currency")
                            .HasMaxLength(3);
                    });
                });

                recipe.OwnsOne(r => r.EstimatedDuration, ed =>
                {
                    // Forzar que el owned use la misma columna PK/FK que ServiceRecipe.recipe_id
                    ed.WithOwner().HasForeignKey("recipe_id");
                    ed.HasKey("recipe_id");

                    // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                    ed.Property<Guid>("recipe_id")
                        .HasColumnName("recipe_id")
                        .IsRequired();

                    ed.Property(d => d.TotalMinutes)
                        .HasColumnName("estimated_duration_minutes");
                });

                recipe.OwnsOne(r => r.WarrantyPeriod, wp =>
                {
                    // Forzar que el owned comparta la misma columna PK/FK que ServiceRecipe.recipe_id
                    wp.WithOwner().HasForeignKey("recipe_id");
                    wp.HasKey("recipe_id");

                    // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                    wp.Property<Guid>("recipe_id")
                        .HasColumnName("recipe_id")
                        .IsRequired();

                    wp.Property(w => w.Value)
                        .HasColumnName("warranty_value");
                    wp.Property(w => w.Unit)
                        .HasConversion<string>()
                        .HasColumnName("warranty_unit")
                        .HasMaxLength(10);
                });

                recipe.Property(r => r.IsActive).HasColumnName("is_active");
                recipe.Property(r => r.TimesRequested).HasColumnName("times_requested");
                recipe.Property(r => r.DeactivationReason).HasColumnName("deactivation_reason").HasMaxLength(200);
            });

            b.Ignore(c => c.DomainEvents);
        });

        // --- ServiceRequest Aggregate ---
        builder.Entity<ServiceRequest>(b =>
        {
            b.ToTable("service_requests");

            b.HasKey(r => r.Id);
            b.Property(r => r.Id)
                .HasConversion(
                    v => v.Id,
                    v => new RequestId(v))
                .HasColumnName("request_id")
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(r => r.HomeownerId)
                .HasConversion(
                    v => v.Id,
                    v => new HomeownerId(v))
                .HasColumnName("homeowner_id")
                .IsRequired();

            b.Property(r => r.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(r => r.IsPriority)
                .HasColumnName("is_priority");

            // PropertySnapshot as owned
            b.OwnsOne(r => r.PropertySnapshot, ps =>
            {
                
                ps.WithOwner().HasForeignKey("request_id");
                ps.HasKey("request_id");

                // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                ps.Property<Guid>("request_id")
                    .HasColumnName("request_id")
                    .IsRequired();
                
                ps.Property(p => p.PropertyId).HasColumnName("property_id");
                ps.Property(p => p.Address).HasColumnName("property_address").HasMaxLength(200);

                ps.OwnsOne(p => p.Geolocation, geo =>
                {
                    // Mantener consistente la FK/PK para el owned anidado
                    geo.WithOwner().HasForeignKey("request_id");
                    geo.HasKey("request_id");

                    geo.Property<Guid>("request_id")
                        .HasColumnName("request_id")
                        .IsRequired();

                    geo.Property(g => g.Latitude).HasColumnName("property_latitude");
                    geo.Property(g => g.Longitude).HasColumnName("property_longitude");
                });
            });

            b.Property(r => r.SelectedRecipeId)
                .HasConversion(
                    v => v != null ? v.Id : (Guid?)null,
                    v => v.HasValue ? new RecipeId(v.Value) : null)
                .HasColumnName("selected_recipe_id");

            b.Property(r => r.SelectedTechnicianId)
                .HasConversion(
                    v => v != null ? v.Id : (Guid?)null,
                    v => v.HasValue ? new TechnicianId(v.Value) : null)
                .HasColumnName("selected_technician_id");

            // ReceiptData as owned
            b.OwnsOne(r => r.ReceiptData, rd =>
            {
                rd.WithOwner().HasForeignKey("request_id");
                rd.HasKey("request_id");

                // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                rd.Property<Guid>("request_id")
                    .HasColumnName("request_id")
                    .IsRequired();

                rd.Property(d => d.ConsumptionKwh)
                    .HasColumnName("receipt_consumption_kwh").HasPrecision(10, 2);
                rd.OwnsOne(d => d.AmountPaid, money =>
                {
                    // Compartir la misma FK/PK con el owner principal
                    money.WithOwner().HasForeignKey("request_id");
                    money.HasKey("request_id");

                    // Mapear la misma propiedad sombra para mantener la columna única
                    money.Property<Guid>("request_id")
                        .HasColumnName("request_id")
                        .IsRequired();

                    money.Property(m => m.Amount)
                        .HasColumnName("receipt_amount_paid")
                        .HasPrecision(18, 2);
                    money.Property(m => m.Currency)
                        .HasColumnName("receipt_currency")
                        .HasMaxLength(3);
                });
                rd.Property(d => d.BillingPeriod).HasColumnName("receipt_billing_period").HasMaxLength(7);
                rd.Property(d => d.ReceiptNumber).HasColumnName("receipt_number").HasMaxLength(50);
            });

            // RequestPreferences as owned
            b.OwnsOne(r => r.Preferences, pref =>
            {
                // Forzar que el owned use la misma columna PK/FK que ServiceRequest.request_id
                pref.WithOwner().HasForeignKey("request_id");
                pref.HasKey("request_id");

                // Propiedad sombra que mapeará la columna ya presente en la tabla principal
                pref.Property<Guid>("request_id")
                    .HasColumnName("request_id")
                    .IsRequired();

                pref.Property<string>("_preferredDatesJson")
                    .HasColumnName("preferred_dates_json")
                    .HasColumnType("jsonb");
                pref.Property(p => p.TimePreference)
                    .HasConversion<string>()
                    .HasColumnName("time_preference")
                    .HasMaxLength(20);
                pref.Property(p => p.ProblemDescription)
                    .HasColumnName("problem_description")
                    .HasMaxLength(1000);
            });

            b.Property(r => r.AssignedServiceId)
                .HasConversion(
                    v => v != null ? v.Id : (Guid?)null,
                    v => v.HasValue ? new ServiceId(v.Value) : null)
                .HasColumnName("assigned_service_id");

            b.Property(r => r.CancellationReason)
                .HasColumnName("cancellation_reason")
                .HasMaxLength(500);

            b.Ignore(r => r.DomainEvents);
        });

        // --- ServiceAssignment Aggregate ---
        builder.Entity<ServiceAssignment>(b =>
        {
            b.ToTable("service_assignments");

            b.HasKey(a => a.Id);
            b.Property(a => a.Id)
                .HasConversion(
                    v => v.Id,
                    v => new ServiceId(v))
                .HasColumnName("service_id")
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(a => a.RequestId)
                .HasConversion(
                    v => v.Id,
                    v => new RequestId(v))
                .HasColumnName("request_id")
                .IsRequired();

            b.Property(a => a.TechnicianId)
                .HasConversion(
                    v => v.Id,
                    v => new TechnicianId(v))
                .HasColumnName("technician_id")
                .IsRequired();

            b.Property(a => a.HomeownerId)
                .HasConversion(
                    v => v.Id,
                    v => new HomeownerId(v))
                .HasColumnName("homeowner_id")
                .IsRequired();

            b.Property(a => a.PropertyId)
                .HasConversion(
                    v => v.Id,
                    v => new PropertyId(v))
                .HasColumnName("property_id")
                .IsRequired();

            // RecipeSnapshot as JSON
            b.OwnsOne(a => a.RecipeSnapshot, rs =>
            {
                rs.Ignore(r => r.ComponentRequirements);
                rs.Ignore(r => r.EstimatedDuration);
                rs.Ignore(r => r.Pricing);
                rs.Ignore(r => r.WarrantyPeriod);


                rs.ToJson("recipe_snapshot");
            });

            // ScheduledSlot as owned
            b.OwnsOne(a => a.ScheduledSlot, slot =>
            {
                // Forzar que el owned use la misma columna PK/FK que el owner (ServiceAssignment.service_id)
                slot.WithOwner().HasForeignKey("service_id");
                slot.HasKey("service_id");

                // Mapear la propiedad sombra al mismo nombre de columna y tipo
                slot.Property<Guid>("service_id")
                    .HasColumnName("service_id")
                    .IsRequired();

                slot.Property(s => s.StartDateTime).HasColumnName("scheduled_start_datetime");
                slot.Property(s => s.EndDateTime).HasColumnName("scheduled_end_datetime");
            });

            b.Property(a => a.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(a => a.IsPriority)
                .HasColumnName("is_priority");

            b.Ignore(a => a.DomainEvents);
        });

        // --- Schedule Entity (keep for backward compatibility) ---
        builder.Entity<Schedule>(b =>
        {
            b.ToTable("schedules");

            b.HasKey(s => s.ScheduleId);
            b.Property(s => s.ScheduleId).HasColumnName("schedule_id").IsRequired();
            b.Property(s => s.TechnicianId).HasColumnName("technician_id").IsRequired();
            b.Property(s => s.Day).HasMaxLength(20).IsRequired();
            b.Property(s => s.StartTime).HasColumnName("start_time").IsRequired();
            b.Property(s => s.EndTime).HasColumnName("end_time").IsRequired();
        });
    }
}
