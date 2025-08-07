using System.Reflection;
// using Cortex.Mediator.DependencyInjection;
// using Cortex.Mediator.Notifications;
using Hampcoders.Electrolink.API.Monitoring.Application.ACL;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EfCore;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Assets.Application.ACL;
using Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;
using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Assets.Interface.ACL;
using Hampcoders.Electrolink.API.IAM.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Services;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL.Services;
using Hampcoders.Electrolink.API.Planning.API.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Planning.API.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;
using Hampcoders.Electrolink.API.Planning.API.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.ACL;
using Hampcoders.Electrolink.API.Planning.Application.ACL;
using Hampcoders.Electrolink.API.Profiles.Application.ACL;
using Hampcoders.Electrolink.API.Profiles.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Profiles.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventPublisher;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Infrastructure.BackgroundServices;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MediatR; 
using System.Reflection; // Necesario para Assembly.GetExecutingAssembly()
using Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;


var builder = WebApplication.CreateBuilder(args);

// General Config
// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null) throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Documento principal
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Hampcoders.ElectrolinkPlatform.API",
        Version     = "v1",
        Description = "Hampcoders Electrolink Platform API",
        Contact     = new OpenApiContact { Name = "Hampcoders", Email = "contact@Hampcoders.com" },
        License     = new OpenApiLicense { Name = "Apache 2.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0") }
    });

    options.EnableAnnotations();

    // JWT Bearer
    /*options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In            = ParameterLocation.Header,
        Description   = "Ingrese el token JWT",
        Name          = "Authorization",
        Type          = SecuritySchemeType.Http,
        Scheme        = "bearer",
        BearerFormat  = "JWT"
    });*/

    /*options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });*/

    /* // Servidor local de desarrollo
    options.AddServer(new OpenApiServer
    {
        Url         = "http://localhost:5055",
        Description = "Development Server"
    });*/ 
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServiceOperationRepository, ServiceOperationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IReportPhotoRepository, ReportPhotoRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

// Domain services for Monitoring
builder.Services.AddScoped<IServiceOperationCommandService, ServiceOperationCommandService>();
builder.Services.AddScoped<IServiceOperationQueryService, ServiceOperationQueryService>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IRatingCommandService, RatingCommandService>();
builder.Services.AddScoped<IRatingQueryService, RatingQueryService>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
builder.Services.AddScoped<IServiceCommandService, ServiceCommandService>();
builder.Services.AddScoped<IServiceQueryService, ServiceQueryService>();
builder.Services.AddScoped<IRequestCommandService, RequestCommandService>();
builder.Services.AddScoped<IRequestQueryService, RequestQueryService>();
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>();
builder.Services.AddScoped<IScheduleQueryService, ScheduleQueryService>();
builder.Services.AddScoped<IMonitoringContextFacade, MonitoringContextFacade>();
builder.Services.AddScoped<ISDPContextFacade, SdpContextFacade>();

// Profiles
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// IAM
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ITechnicianInventoryRepository, TechnicianInventoryRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IComponentTypeRepository, ComponentTypeRepository>();
builder.Services.AddScoped<IPropertyCommandService, PropertyCommandService>();
builder.Services.AddScoped<ITechnicianInventoryCommandService, TechnicianInventoryCommandService>();
builder.Services.AddScoped<IComponentCommandService, ComponentCommandService>();
builder.Services.AddScoped<IComponentTypeCommandService, ComponentTypeCommandService>();
builder.Services.AddScoped<IPropertyQueryService, PropertyQueryService>();
builder.Services.AddScoped<ITechnicianInventoryQueryService, TechnicianInventoryQueryService>();
builder.Services.AddScoped<IComponentQueryService, ComponentQueryService>();
builder.Services.AddScoped<IComponentTypeQueryService, ComponentTypeQueryService>();

// Assets ACL
builder.Services.AddScoped<IAssetsContextFacade, AssetsContextFacade>();
builder.Services.AddScoped<ExternalAssetService>(); 

builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddHostedService<OutboxProcessorBackgroundService>();


// Add Cortex Mediator for Event Handling
builder.Services.AddMediatR(cfg => {
        // Opcional: Si tienes comportamientos de pipeline (LoggingCommandBehavior), configúralos aquí.
        // MediatR tiene su propia interfaz IPipelineBehavior.
        // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingCommandBehavior<,>)); 
    }, 
    // Escanear el ensamblado actual (donde está Program.cs) para comandos, queries y manejadores.
    Assembly.GetExecutingAssembly(), 
    
    // Escanear el ensamblado de Assets.Application para Event Handlers.
    // Asegúrate de que Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers.ComponentCreatedEventHandler
    // sea un tipo que exista en ese ensamblado.
    typeof(Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers.ComponentCreatedEventHandler).Assembly
    
    // Si tienes otros ensamblados con comandos/queries/eventos/manejadores, añádelos aquí:
    // typeof(SomeOtherAssemblyMarkerClass).Assembly
);

var app = builder.Build();

// DB Init
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Middleware
/* if (app.Environment.IsDevelopment())
{
    // Uncomment the following lines to enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI();
} */

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllPolicy");
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Uncomment the following line to enable OpenAPI documentation (Development Server)
app.Urls.Add("http://*:8088");
// app.Urls.Add("http://*:8080");

app.Run();
