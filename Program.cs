using System.Text;
using Hampcoders.Electrolink.API.Monitoring.Application.ACL;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Assets.Application.ACL;
using Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.IAM.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MediatR; 
using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Services;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using ExternalIamServiceForSubscriptionsBC = Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices.ExternalIamService;
using ExternalIamServiceForProfilesBC = Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices.ExternalIamService;
using TokenService = Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Services.TokenService;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

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

var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];

if (!string.IsNullOrEmpty(stripeSecretKey))
{
    // Configura la clave API globalmente para Stripe.net
    StripeConfiguration.ApiKey = stripeSecretKey;
}
else
{
    // Manejo de error si la clave secreta no se encuentra
    throw new InvalidOperationException("Stripe SecretKey no configurada. No se pueden hacer llamadas de servidor.");
}

var stripeConfig = builder.Configuration
                       .GetSection(StripeSettings.SectionName)
                       .Get<StripeSettings>() 
                   ?? throw new InvalidOperationException("Stripe configuration is missing");

stripeConfig.Validate();

builder.Services.AddScoped<SessionService>();
builder.Services.AddSingleton<StripeClientFactory>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddSingleton<StripeSettings>(sp => sp.GetRequiredService<IOptions<StripeSettings>>().Value);
// 3. Registrar Payment Gateway Service (Domain Service → Infrastructure Implementation)
builder.Services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
builder.Services.AddScoped<IWebhookEventRepository, WebhookEventRepository>();

// 4. Registrar Webhook Infrastructure
//builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<StripeWebhookValidator>();
builder.Services.AddScoped<StripeWebhookEventProcessor>();

// 5. Registrar CoICheckoutCommandService, and Services
//builder.Services.AddScoped<ICheckoutCommandService,CheckoutCommandService>();
builder.Services.AddScoped<StripeWebhookCommandService>();
// 6. Registrar Repositories


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

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
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In            = ParameterLocation.Header,
        Description   = "Ingrese el token JWT",
        Name          = "Authorization",
        Type          = SecuritySchemeType.Http,
        Scheme        = "bearer",
        BearerFormat  = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });

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



// Dependency Injection

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<StripeEventMapper>();
builder.Services.AddScoped<IServiceOperationRepository, ServiceOperationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IReportPhotoRepository, ReportPhotoRepository>();


// Domain services for Monitoring
builder.Services.AddScoped<IServiceOperationCommandService, ServiceOperationCommandService>();
builder.Services.AddScoped<IServiceOperationQueryService, ServiceOperationQueryService>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IRatingCommandService, RatingCommandService>();
builder.Services.AddScoped<IRatingQueryService, RatingQueryService>();

builder.Services.AddScoped<IMonitoringContextFacade, MonitoringContextFacade>();

// Subscriptions and Payments Bounded Context
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
builder.Services.AddScoped<IPaymentTransactionQueryService,PaymentTransactionQueryService>();
builder.Services.AddScoped<IPaymentTransactionCommandService,PaymentTransactionCommandService>();
builder.Services.AddScoped<ExternalIamServiceForSubscriptionsBC>();
builder.Services.AddScoped<ExternalProfileService>();

// Planning
builder.Services.AddScoped<IServiceAssignmentRepository, ServiceAssignmentRepository>();
builder.Services.AddScoped<IServiceCatalogRepository, ServiceCatalogRepository>();
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

builder.Services.AddScoped<IServiceAssignmentCommandService, ServiceAssignmentCommandService>();
builder.Services.AddScoped<IServiceRequestCommandService, ServiceRequestCommandService>();
builder.Services.AddScoped<IServiceCatalogCommandService, ServiceCatalogCommandService>();

builder.Services.AddScoped<IServiceAssignmentQueryService, ServiceAssignmentQueryService>();
builder.Services.AddScoped<IServiceRequestQueryService, ServiceRequestQueryService>();
builder.Services.AddScoped<IServiceCatalogQueryService, ServiceCatalogQueryService>();

builder.Services.AddScoped<Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices.ExternalAssetsService>();
builder.Services.AddScoped<Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices.ExternalProfilesService>();
builder.Services.AddScoped<Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices.ExternalSubscriptionsService>();

// Profiles
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();
builder.Services.AddScoped<IProfileUniquenessChecker, ProfileUniquenessChecker>();
builder.Services.AddScoped<ExternalIamServiceForProfilesBC>();
builder.Services.AddScoped<ExternalAssetService>();

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
builder.Services.AddScoped<IPropertyPortfolioCommandService, PropertyPortfolioCommandService>();
builder.Services.AddScoped<IPropertyPortfolioQueryService, PropertyPortfolioQueryService>();
builder.Services.AddScoped<IPropertyPortfolioRepository, PropertyPortfolioRepository>();

// Assets ACL
builder.Services.AddScoped<IAssetsContextFacade, AssetsContextFacade>();
builder.Services.AddScoped<ExternalAssetService>(); 

builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddHostedService<OutboxProcessorBackgroundService>();


// Add Cortex Mediator for Event Handling
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
    .ToArray();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Carga la clave secreta desde la configuración (asegúrate de que TokenSettings.Secret esté configurado)
        var secret = builder.Configuration["TokenSettings:Secret"] ?? throw new InvalidOperationException("TokenSettings:Secret not configured.");
        var key = Encoding.ASCII.GetBytes(secret);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // Valida la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(key), // Usa tu clave secreta
            ValidateIssuer = false, // Puedes establecer esto en true si tienes un emisor de tokens específico
            ValidateAudience = false, // Puedes establecer esto en true si tienes una audiencia de tokens específica
            ValidateLifetime = true, // Valida la fecha de expiración del token
            ClockSkew = TimeSpan.Zero // No permite desviación del reloj para la expiración
        };
    });
//builder.Services.AddMediatR(cfg => { }, assemblies);

builder.Services.AddMediatR(typeof(StripeEventHandler).Assembly);

    
var app = builder.Build();

// DB Init
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    // Uncomment the following lines to enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRequestAuthorization();
app.UseAuthorization();
app.MapControllers();

// Uncomment the following line to enable OpenAPI documentation (Development Server)
app.Urls.Add("http://*:8088");
// app.Urls.Add("http://*:8080");
builder.Logging.AddConsole();
app.Run();
