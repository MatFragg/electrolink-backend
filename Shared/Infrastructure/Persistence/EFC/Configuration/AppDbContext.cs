using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;


using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Entities;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ServiceOperation> ServiceOperations { get; set; }
    
    public DbSet<Property> Properties { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<TechnicianInventory> TechnicianInventories { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentStock> ComponentStocks { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }
    
    // Planning Bounded Context (Refactored with Tactical DDD)
    //public DbSet<Service> Services { get; set; } // OLD - replaced by ServiceCatalog
    //public DbSet<Request> Requests { get; set; } // OLD - replaced by ServiceRequest
    public DbSet<ServiceCatalog> ServiceCatalogs { get; set; }
    public DbSet<ServiceRecipe> ServiceRecipes { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<ServiceAssignment> ServiceAssignments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("postgis");

        builder.ApplyIamConfiguration();
        builder.ApplyProfilesConfiguration();
        builder.ApplyAssetsConfiguration();
        builder.ApplyServiceDesignAndPlanningConfiguration();
        builder.ApplyMonitoringConfiguration();
        builder.ApplySubscriptionsConfiguration();
        builder.UseSnakeCaseNamingConvention();
    }
    
    
}