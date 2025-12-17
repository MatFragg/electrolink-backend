# Estructura de Carpetas: Stripe Integration en Subscriptions Context

```
📁 Subscriptions/                                    # Tu Bounded Context existente
│
├── 📁 Domain/
│   ├── 📁 Model/
│   │   ├── 📁 Aggregates/
│   │   │   ├── Subscription.cs                     ✅ Ya existe
│   │   │   ├── Plan.cs                             ✅ Ya existe
│   │   │   └── PaymentTransaction.cs               ✅ Ya existe
│   │   │
│   │   ├── 📁 ValueObjects/
│   │   │   ├── Benefit
│   │   │   ├── EMonetizationType
│   │   │   ├── EPaymentStatus
│   │   │   ├── ESubscriptionStatus
│   │   │   ├── EUserRole
│   │   │   ├── PlanId
│   │   │   ├── SubscriptionId
│   │   │   ├── UserId
│   │   │   ├── StripeCustomerId.cs                 🆕 NUEVO
│   │   │   ├── StripeSubscriptionId.cs             🆕 NUEVO
│   │   │   ├── StripePriceId.cs                    🆕 NUEVO
│   │   │   ├── StripePaymentIntentId.cs            🆕 NUEVO
│   │   │   └── WebhookEventId.cs                   🆕 NUEVO
│   │   │
│   │   ├── 📁 Events/
│   │   │   ├── 📁 Domain/                          ✅ Ya existen
│   │   │   │   ├── PaymentProcessedEvent.cs
│   │   │   │   ├── SubscriptionStatusChangedEvent.cs
│   │   │   │   └── SubscriptionPlanChangedEvent.cs
│   │   │   │
│   │   │   └── 📁 Integration/                     🆕 NUEVA CARPETA
│   │   │       ├── SubscriptionActivatedIntegrationEvent.cs
│   │   │       ├── SubscriptionCancelledIntegrationEvent.cs
│   │   │       ├── PaymentFailedIntegrationEvent.cs
│   │   │       ├── TrialEndingSoonIntegrationEvent.cs
│   │   │       └── PlanChangedIntegrationEvent.cs
│   │   │
│   │   └── 📁 Commands/
│   │       ├── SyncSubscriptionFromStripeCommand.cs    🆕 NUEVO
│   │       └── HandleStripeWebhookCommand.cs           🆕 NUEVO
│   │
│   ├── 📁 Services/
│   │   ├── IPaymentGatewayService.cs                   🆕 NUEVO (Interfaz)
│   │   └── IWebhookEventStore.cs                       🆕 NUEVO
│   │
│   └── 📁 Repositories/
│       └── IWebhookEventRepository.cs                  🆕 NUEVO
│
├── 📁 Application/
│   ├── 📁 Internal/
│   │   ├── 📁 CommandServices/
│   │   │   ├── SubscriptionCommandService.cs          ✅ Ya existe (MODIFICAR)
│   │   │   ├── PaymentTransactionCommandService.cs    ✅ Ya existe (MODIFICAR)
│   │   │   ├── StripeWebhookCommandService.cs         🆕 NUEVO
│   │   │   └── StripeSyncCommandService.cs            🆕 NUEVO
│   │   │
│   │   ├── 📁 EventHandlers/
│   │   │   ├── 📁 Domain/                             ✅ Ya existen
│   │   │   │   └── PaymentProcessedEventHandler.cs
│   │   │   │
│   │   │   └── 📁 Integration/                        🆕 NUEVA CARPETA
│   │   │       ├── SubscriptionActivatedIntegrationEventHandler.cs
│   │   │       └── PaymentFailedIntegrationEventHandler.cs
│   │   │
│   │   └── 📁 OutboundServices/
│   │       ├── ExternalProfileService.cs              ✅ Ya existe
│   │       ├── ExternalIamService.cs                  ✅ Ya existe
│   │       └── INotificationService.cs                🆕 NUEVO (para emails)
│   │
│   └── 📁 ACL/
│       └── SubscriptionsContextFacade.cs              🆕 NUEVO (para otros contextos)
│
├── 📁 Infrastructure/
│   ├── 📁 Persistence/
│   │   └── 📁 EFC/
│   │       ├── 📁 Configurations/
│   │       │   └── WebhookEventConfiguration.cs       🆕 NUEVO
│   │       │
│   │       └── 📁 Repositories/
│   │           └── WebhookEventRepository.cs          🆕 NUEVO
│   │
│   └── 📁 PaymentGateway/                             🆕 NUEVA CARPETA COMPLETA
│       └── 📁 Stripe/
│           ├── StripePaymentGatewayService.cs         🆕 Implementación del servicio
│           ├── StripeClientFactory.cs                 🆕 Factory para cliente Stripe
│           ├── StripeEventMapper.cs                   🆕 Mapea eventos Stripe → Domain
│           ├── StripeConfiguration.cs                 🆕 Settings de Stripe
│           │
│           ├── 📁 Models/                             🆕 DTOs internos de Stripe
│           │   ├── StripeCustomerDto.cs
│           │   ├── StripeSubscriptionDto.cs
│           │   └── StripeInvoiceDto.cs
│           │
│           └── 📁 Webhooks/
│               ├── StripeWebhookValidator.cs          🆕 Valida signatures
│               └── StripeWebhookEventProcessor.cs     🆕 Procesa eventos
│
└── 📁 Interfaces/
    └── 📁 REST/
        ├── SubscriptionsController.cs                 ✅ Ya existe (MODIFICAR)
        ├── StripeWebhooksController.cs                🆕 NUEVO (Endpoint webhooks)
        ├── CheckoutController.cs                      🆕 NUEVO (Crear sesiones pago)
        │
        ├── 📁 Resources/
        │   ├── CreateCheckoutSessionResource.cs       🆕 NUEVO
        │   ├── StripeWebhookEventResource.cs          🆕 NUEVO
        │   └── SubscriptionStatusResource.cs          🆕 NUEVO
        │
        └── 📁 Transform/
            ├── StripeWebhookEventAssembler.cs         🆕 NUEVO
            └── CheckoutSessionAssembler.cs            🆕 NUEVO
```

---

## 📦 Paquetes NuGet Necesarios

```bash
# En Subscriptions.csproj
dotnet add package Stripe.net --version 45.3.0
dotnet add package Microsoft.Extensions.Hosting.Abstractions
```

---

## 🔑 Leyenda

- ✅ **Ya existe** - Archivo ya implementado en tu código
- 🆕 **NUEVO** - Archivo que debes crear
- **MODIFICAR** - Archivo existente que requiere cambios

---

## 📝 Notas Importantes

### 1. **Separación de Responsabilidades**

- `Domain/Services/IPaymentGatewayService.cs` → Interfaz (contrato del dominio)
- `Infrastructure/PaymentGateway/Stripe/` → Implementación técnica (detalle)

### 2. **Domain Events vs Integration Events**

- **Domain Events** (`Domain/Model/Events/Domain/`) → Comunicación interna (mismo proceso)
- **Integration Events** (`Domain/Model/Events/Integration/`) → Comunicación externa (otros contextos)

### 3. **Outbox Pattern En Shared Kernel**

- Garantiza entrega de Integration Events
- Background Service procesa mensajes pendientes

```
using System.Text.Json;
using MediatR;
using Cortex.Mediator.Notifications;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.BackgroundServices;

public class OutboxProcessorBackgroundService : BackgroundService
{
    private readonly ILogger<OutboxProcessorBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider; 

    public OutboxProcessorBackgroundService(ILogger<OutboxProcessorBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor Background Service running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutboxMessagesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); 
        }

        _logger.LogInformation("Outbox Processor Background Service stopped.");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var outboxMessages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(100)
            .ToListAsync(stoppingToken);

        var now = DateTime.UtcNow;

        foreach (var message in outboxMessages)
        {
            if (stoppingToken.IsCancellationRequested) return;

            try
            {
                var integrationEventType = Type.GetType(message.Type);
                if (integrationEventType is null)
                    throw new InvalidOperationException($"No se pudo resolver el tipo: {message.Type}");

                // ¡CAMBIO CLAVE AQUÍ! Cast a IIntegrationEvent
                var integrationEvent = JsonSerializer.Deserialize(message.Content, integrationEventType) as IIntegrationEvent;
                if (integrationEvent is null)
                    throw new InvalidOperationException($"No se pudo deserializar el contenido para el tipo: {message.Type}");

                // MediatR Publish espera INotification, y IIntegrationEvent hereda de IEvent, que hereda de INotification.
                // Así que el publish es correcto.
                await mediator.Publish(integrationEvent, stoppingToken);

                message.ProcessedOnUtc = now;
                message.Error = null;
                _logger.LogInformation($"Publicado mensaje outbox (MediatR): {integrationEvent.GetType().Name} - {integrationEvent.EventId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error procesando mensaje outbox ID {message.Id}.");
                message.Error = ex.Message;
                message.ProcessedOnUtc = now;
            }
        }

        if (outboxMessages.Count > 0)
            await dbContext.SaveChangesAsync(stoppingToken);
    }
}

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOnUtc { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; 
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}
```

### 4. **Webhook Handling**

- Controller expone endpoint público: `/api/v1/stripe/webhooks`
- Validación de signature de Stripe
- Idempotencia con `WebhookEventRepository`

### 5. **ACL (Anti-Corruption Layer)**

- `Application/ACL/SubscriptionsContextFacade.cs`
- Otros contextos (IAM, Profiles, Dashboard) consumen a través del ACL
- Nunca acceden directamente a repositorios de Subscriptions

### 6. Domain / Application / Repository

```
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a subscription plan that users can subscribe to. This is the root aggregate for managing plans.
/// </summary>
public class Plan
{
    /// <summary>
    /// Unique identifier for the plan.
    /// </summary>
    public PlanId Id { get; private set; }

    /// <summary>
    /// The name of the plan (e.g., "Freemium", "Premium Propietario").
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A detailed description of the plan.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// The price of the plan.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// The currency of the plan's price (e.g., "USD").
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// The monetization type (e.g., Free, Monthly, Annually).
    /// </summary>
    public EMonetizationType MonetizationType { get; private set; }

    /// <summary>
    /// Indicates if this is a default plan (e.g., the default Freemium plan).
    /// </summary>
    public bool IsDefault { get; private set; }

    /// <summary>
    /// The target role for this plan (Homeowner, Technician, or All).
    /// </summary>
    public EUserRole TargetRole { get; private set; }

    /// <summary>
    /// A collection of benefits included in this plan.
    /// </summary>
    public List<Benefit> Benefits { get; private set; } = new List<Benefit>();
    
    /// <summary>
    /// The ID of this plan in Stripe (Price ID). Can be null for free plans.
    /// </summary>
    public string? StripePriceId { get; private set; }
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private Plan() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Plan"/> class.
    /// </summary>
    /// <param name="name">The name of the plan.</param>
    /// <param name="description">The description of the plan.</param>
    /// <param name="price">The price of the plan.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="monetizationType">The monetization type.</param>
    /// <param name="targetRole">The target user role for this plan.</param>
    /// <param name="isDefault">Indicates if it's a default plan.</param>
    /// <param name="benefits">The list of benefits.</param>
    /// <param name="stripePriceId">Optional Stripe Price ID associated with this plan.</param>
    public Plan(string name, string description, decimal price, string currency, EMonetizationType monetizationType, EUserRole targetRole, bool isDefault, List<Benefit> benefits, string? stripePriceId = null)
    {
        Id = new PlanId(Guid.NewGuid());
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
        TargetRole = targetRole;
        Benefits = benefits ?? new List<Benefit>();
        StripePriceId = stripePriceId;
        
        _domainEvents.Add(new PlanCreatedEvent(
            Id.Value,
            Name,
            Price,
            MonetizationType,
            TargetRole,
            IsDefault,
            Benefits,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the details of the plan.
    /// </summary>
    /// <param name="name">The new name.</param>
    /// <param name="description">The new description.</param>
    /// <param name="price">The new price.</param>
    /// <param name="currency">The new currency.</param>
    /// <param name="monetizationType">The new monetization type.</param>
    /// <param name="targetRole">The new target user role.</param>
    /// <param name="isDefault">The new default status.</param>
    /// <param name="benefits">The updated list of benefits.</param>
    /// <param name="stripePriceId">Optional new Stripe Price ID associated with this plan.</param>
    public void UpdateDetails(string name, string description, decimal price, string currency, EMonetizationType monetizationType, EUserRole targetRole, bool isDefault, List<Benefit> benefits, string? stripePriceId = null)
    {
        if (Name == name && Description == description && Price == price && Currency == currency &&
            MonetizationType == monetizationType && IsDefault == isDefault && TargetRole == targetRole &&
            StripePriceId == stripePriceId &&
            Benefits.SequenceEqual(benefits ?? new List<Benefit>()))
        {
            return; 
        }
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
        TargetRole = targetRole;
        Benefits = benefits ?? new List<Benefit>();
        StripePriceId = stripePriceId;
        
        _domainEvents.Add(new PlanDetailsUpdatedEvent(
            Id.Value, 
            name, 
            description, 
            price, 
            currency, 
            monetizationType, 
            isDefault, 
            targetRole,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Adds a benefit to the plan.
    /// </summary>
    /// <param name="benefit">The benefit to add.</param>
    public void AddBenefit(Benefit benefit)
    {
        Benefits.Add(benefit);
    }

    /// <summary>
    /// Removes a benefit from the plan.
    /// </summary>
    /// <param name="benefitType">The type of the benefit to remove.</param>
    public void RemoveBenefit(string benefitType)
    {
        Benefits.RemoveAll(b => b.Type == benefitType);
    }

    /// <summary>
    /// Checks if the plan has a specific benefit.
    /// </summary>
    /// <param name="benefitType">The type of benefit to check.</param>
    /// <returns>True if the plan has the benefit, false otherwise.</returns>
    public bool HasBenefit(string benefitType)
    {
        return Benefits.Exists(b => b.Type == benefitType);
    }

    /// <summary>
    /// Gets a specific benefit from the plan.
    /// </summary>
    /// <param name="benefitType">The type of benefit to retrieve.</param>
    /// <returns>The <see cref="Benefit"/> if found, otherwise null.</returns>
    public Benefit? GetBenefit(string benefitType)
    {
        if (string.IsNullOrEmpty(benefitType))
            return null;

        return Benefits.Find(b => b.Type == benefitType);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a record of a payment transaction.
/// </summary>
public class PaymentTransaction
{
    /// <summary>
    /// Unique identifier for the payment transaction.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The ID of the subscription associated with this transaction.
    /// </summary>
    public SubscriptionId SubscriptionId { get; private set; }

    /// <summary>
    /// The amount of the transaction.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// The currency of the transaction.
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// The date and time when the transaction occurred.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// The status of the transaction (e.g., Success, Failed, Pending).
    /// </summary>
    public EPaymentStatus Status { get; private set; }

    /// <summary>
    /// The identifier from the payment gateway (e.g., Stripe Charge ID).
    /// </summary>
    public string GatewayTransactionId { get; private set; }

    /// <summary>
    /// Optional message or reason for the transaction status (e.g., error message).
    /// </summary>
    public string Message { get; private set; }
    
    
    /// <summary>
    ///  Domain events associated with this aggregate.
    /// </summary>
    private readonly List<IEvent> _domainEvents = new();
    
    /// <summary>
    /// Read-only collection of domain events.
    /// </summary>
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    
    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private PaymentTransaction() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransaction"/> class.
    /// </summary>
    /// <param name="subscriptionId">The ID of the associated subscription.</param>
    /// <param name="amount">The transaction amount.</param>
    /// <param name="currency">The transaction currency.</param>
    /// <param name="transactionDate">The date and time of the transaction.</param>
    /// <param name="status">The status of the transaction.</param>
    /// <param name="gatewayTransactionId">The payment gateway's transaction ID.</param>
    /// <param name="message">Optional message.</param>
    public PaymentTransaction(SubscriptionId subscriptionId, decimal amount, string currency, DateTime transactionDate, EPaymentStatus status, string gatewayTransactionId, string message = "")
    {
        Id = Guid.NewGuid();
        SubscriptionId = subscriptionId;
        Amount = amount;
        Currency = currency;
        TransactionDate = transactionDate;
        Status = status;
        GatewayTransactionId = gatewayTransactionId;
        Message = message;
        
        _domainEvents.Add(new PaymentProcessedEvent(
            Id,
            subscriptionId.Value,
            amount,
            status,
            gatewayTransactionId,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the status and message of the payment transaction.
    /// </summary>
    /// <param name="newStatus">The new payment status.</param>
    /// <param name="newMessage">An updated message.</param>
    public void UpdateStatus(EPaymentStatus newStatus, string newMessage = "")
    {
        var oldStatus = Status;
        if (oldStatus == newStatus && Message == newMessage) return;
        Status = newStatus;
        Message = newMessage;
        
        _domainEvents.Add(new PaymentStatusUpdatedEvent(
            Id,
            SubscriptionId.Value,
            oldStatus,
            newStatus,
            DateTime.UtcNow));
    }
    
    /// <summary>
    /// Clears all domain events from the transaction's event list.
    ///</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }   
}


using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a user's subscription. This is the root aggregate for managing subscription lifecycle.
/// </summary>
public class Subscription
{
    /// <summary>
    /// Unique identifier for the subscription.
    /// </summary>
    public SubscriptionId Id { get; private set; }
    
    /// <summary>
    /// The ID of the user associated with this subscription.
    /// </summary>
    public UserId UserId { get; private set; }

    /// <summary>
    /// The ID of the plan to which the user is subscribed.
    /// </summary>
    public PlanId PlanId { get; private set; }

    /// <summary>
    /// The current status of the subscription.
    /// </summary>
    public ESubscriptionStatus Status { get; private set; }

    /// <summary>
    /// The date when the subscription started.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// The date when the current subscription period is scheduled to end.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// The date when a cancelled subscription will actually cease to provide benefits.
    /// </summary>
    public DateTime? CancellationEffectiveDate { get; private set; }

    /// <summary>
    /// The date when the trial period (if any) ends.
    /// </summary>
    public DateTime? TrialEndsAt { get; private set; }

    /// <summary>
    /// Stripe's unique identifier for the customer.
    /// </summary>
    public string StripeCustomerId { get; private set; }

    /// <summary>
    /// Stripe's unique identifier for the subscription.
    /// </summary>
    public string StripeSubscriptionId { get; private set; }
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Current usage counter for limited benefits (e.g., number of service requests).
    /// </summary>
    public int CurrentUsage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subscription"/> class for a new subscription.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="planId">The ID of the plan.</param>
    /// <param name="startDate">The start date of the subscription.</param>
    /// <param name="endDate">The end date of the subscription.</param>
    /// <param name="stripeCustomerId">Stripe's customer ID.</param>
    /// <param name="stripeSubscriptionId">Stripe's subscription ID.</param>
    /// <param name="status">The initial status of the subscription.</param>
    /// <param name="trialEndsAt">Optional trial end date.</param>
    public Subscription(UserId userId, PlanId planId, DateTime startDate, DateTime endDate, string stripeCustomerId,
        string stripeSubscriptionId, ESubscriptionStatus status, DateTime? trialEndsAt = null)
    {
        Id = new SubscriptionId(Guid.NewGuid());
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        StripeCustomerId = stripeCustomerId;
        StripeSubscriptionId = stripeSubscriptionId;
        TrialEndsAt = trialEndsAt;
        CurrentUsage = 0;
    }

    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private Subscription()
    {
    }   
    
    /// <summary>
    /// Adds a domain event to the subscription's event list.
    /// </summary>
    /// <param name="domainEvent"></param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    /// <summary>
    /// Clears all domain events from the subscription's event list.
    ///</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Updates the status of the subscription.
    /// </summary>
    /// <param name="newStatus">The new status to set.</param>
    public void UpdateStatus(ESubscriptionStatus newStatus)
    { 
        var oldStatus = Status;
        if (oldStatus == newStatus) return;
            
        Status = newStatus;
        _domainEvents.Add(new SubscriptionStatusChangedEvent(
            Id.Value, 
            UserId, 
            oldStatus, 
            newStatus, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Schedules the subscription for cancellation, making it effective at the end of the current period.
    /// </summary>
    /// <param name="effectiveDate">The date when the cancellation becomes effective.</param>
    public void ScheduleCancellation(DateTime effectiveDate)
    {
        CancellationEffectiveDate = effectiveDate;
        // Optionally, change status to CancellationScheduled if needed
        // Status = ESubscriptionStatus.CancellationScheduled;
        
        if (Status != ESubscriptionStatus.Cancelled) 
            UpdateStatus(ESubscriptionStatus.Cancelled);
    }

    /// <summary>
    /// Activates a trial period for the subscription.
    /// </summary>
    /// <param name="trialEndDate">The date when the trial period ends.</param>
    public void ActivateTrial(DateTime trialEndDate)
    {
        var oldStatus = Status;
        Status = ESubscriptionStatus.Trial;
        TrialEndsAt = trialEndDate;
        CurrentUsage = 0;    // Reset usage for trial
        
        _domainEvents.Add(new SubscriptionStatusChangedEvent(
            Id.Value, 
            UserId, 
            oldStatus, 
            Status, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Increases the current usage count for a limited benefit.
    /// </summary>
    public void IncrementUsage()
    {
        CurrentUsage++;
    }

    /// <summary>
    /// Resets the current usage count for a limited benefit.
    /// </summary>
    public void ResetUsage()
    {
        CurrentUsage = 0;
    }

    /// <summary>
    /// Updates the Stripe subscription ID.
    /// </summary>
    /// <param name="newStripeSubscriptionId">The new Stripe subscription ID.</param>
    public void UpdateStripeSubscriptionId(string newStripeSubscriptionId)
    {
        StripeSubscriptionId = newStripeSubscriptionId;
    }

    /// <summary>
    /// Updates the subscription's plan.
    /// </summary>
    /// <param name="newPlanId">The ID of the new plan.</param>
    /// <param name="newEndDate">The new end date for the subscription.</param>
    public void ChangePlan(PlanId newPlanId, DateTime newEndDate)
    {
        var oldPlanId = PlanId;
        PlanId = newPlanId;
        EndDate = newEndDate;
        CurrentUsage = 0; // Reset usage on plan change
        TrialEndsAt = null; // Remove trial if changing plan
        // Raise a domain event
        _domainEvents.Add(new SubscriptionPlanChangedEvent(
            Id.Value, 
            UserId, 
            oldPlanId, 
            newPlanId, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the subscription's end date.
    /// </summary>
    /// <param name="newEndDate">The new end date.</param>
    public void UpdateEndDate(DateTime newEndDate)
    {
        EndDate = newEndDate;
    }
    /// <summary>
    /// Updates the subscription's trial end date.
    /// </summary>
    /// <param name="newTrialEndsAt">The new trial end date, or null if trial is over/removed.</param>
    public void UpdateTrialEndsAt(DateTime? newTrialEndsAt)
    {
        TrialEndsAt = newTrialEndsAt;
        // Optionally, raise a domain event if this change is significant.
    }
    
}

using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IPaymentTransactionRepository"/>.
/// </summary>
public class PaymentTransactionRepository(AppDbContext context) : BaseRepository<PaymentTransaction, Guid>(context), IPaymentTransactionRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<PaymentTransaction>> ListBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await Context.Set<PaymentTransaction>()
            .Where(pt => pt.SubscriptionId.Value == subscriptionId)
            .ToListAsync();
    }

}


using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class PlanRepository(AppDbContext context)
    : BaseRepository<Plan, PlanId>(context), IPlanRepository
{
    
    /*public new async Task<Plan?> FindByIdAsync(Guid id)
    {
        return await FindByIdAsync(new PlanId(id));
    }*/
    
    /// <inheritdoc/>
    public async Task<Plan?> FindDefaultAsync()
    {
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(p => p.IsDefault);
    }

    /// <inheritdoc/>
    public async Task<Plan?> FindDefaultPlanByRoleAsync(EUserRole role)
    {
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.IsDefault && (p.TargetRole == role || p.TargetRole == EUserRole.All));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Plan>> ListPlansByRoleAsync(EUserRole role)
    {
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .Where(p => p.TargetRole == role || p.TargetRole == EUserRole.All)
            .ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<Plan?> FindByStripePriceIdAsync(string stripePriceId)
    {
        // Asume que el agregado Plan tiene una propiedad pública llamada StripePriceId.
        // Si no la tiene, tendrás que añadirla al agregado Plan y a la configuración de EF Core.
        return await Context.Set<Plan>()
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.StripePriceId == stripePriceId);
    }
}

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription, SubscriptionId>(context), ISubscriptionRepository
{
    
    /// <inheritdoc/>
    public async Task<Subscription?> FindBySubscriptionIdAsync(SubscriptionId id)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Subscription?> FindByUserIdAsync(UserId userId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Subscription>> ListActiveAsync()
    {
        return await Context.Set<Subscription>()
            .Where(s => s.Status == ESubscriptionStatus.Active || s.Status == ESubscriptionStatus.Trial)
            .ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<Subscription?> FindByStripeCustomerIdAsync(string stripeCustomerId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeCustomerId == stripeCustomerId);
    }

    /// <inheritdoc/>
    public async Task<Subscription?> FindByStripeSubscriptionIdAsync(string stripeSubscriptionId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSubscriptionId);
    }
}

using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class WebhookEventRepository(AppDbContext context) : BaseRepository<WebhookEvent, WebhookEventId>(context), IWebhookEventRepository
{

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}

using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class PaymentTransactionCommandService(IPaymentTransactionRepository paymentTransactionRepository,ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IMediator mediator) : IPaymentTransactionCommandService
{
    /// <inheritdoc/>
    public async Task<Guid> Handle(ProcessPaymentCommand command)
    {
        var subscriptionId = new SubscriptionId(command.SubscriptionId);
        var subscription = await subscriptionRepository.FindBySubscriptionIdAsync(subscriptionId);
        if (subscription == null)
            throw new ArgumentException($"Subscription with ID {subscriptionId} not found for payment processing.");
        
        var transaction = new PaymentTransaction(
            subscriptionId,
            command.Amount ?? 0,
            command.Currency,
            command.TransactionDate,
            command.Status,
            command.GatewayTransactionId,
            command.Message
        );  

        await paymentTransactionRepository.AddAsync(transaction);
        var oldSubscriptionStatus = subscription.Status;

        // Update subscription status based on payment result
        if (command.Status == EPaymentStatus.Success)
        {
            subscription.UpdateStatus(ESubscriptionStatus.Active);
            // Reset cancellation if a pending payment succeeded        
            subscription.UpdateEndDate(subscription.EndDate.AddMonths(1)); // Example: extend for one month
        }
        else if (command.Status == EPaymentStatus.Failed)
        {
            subscription.UpdateStatus(ESubscriptionStatus.PaymentDue); // Enter payment due state for retries
        }
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in transaction.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        transaction.ClearDomainEvents();
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        return transaction.Id; // Return the ID
    }
}


using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class PlanCommandService(IPlanRepository planRepository, IUnitOfWork unitOfWork, IMediator mediator) : IPlanCommandService
{
    public async Task<Guid> Handle(CreatePlanCommand command)
    {
        var plan = new Plan(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.TargetRole,
            command.IsDefault,
            command.Benefits,
            command.StripePriceId
        );

        await planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();

        // Publish domain event
        foreach (var domainEvent in plan.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        plan.ClearDomainEvents();
        
        return plan.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(UpdatePlanCommand command)
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null) return null;

        plan.UpdateDetails(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.TargetRole,
            command.IsDefault,
            command.Benefits
        );

        planRepository.Update(plan); // Changed to void
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in plan.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        plan.ClearDomainEvents();   
        
        return plan.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task Handle(DeletePlanCommand command) // Void return
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null) throw new ArgumentException($"Plan with ID {command.PlanId} not found.");

        planRepository.Remove(plan); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., PlanDeletedEvent)  if needed, notify meaningful changes.
        // await mediator.Publish(new PlanDeletedEvent(command.PlanId, DateTime.UtcNow));

    }
}


using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class StripeCheckoutCommandService(IPaymentGatewayService paymentGateway, IPlanRepository planRepository, ISubscriptionRepository subscriptionRepository, ExternalIamService externalIamService, IUnitOfWork unitOfWork, ILogger<StripeCheckoutCommandService> logger) : IStripeCheckoutCommandService
{
    /// <summary>
    /// Creates a Stripe Checkout session to subscribe the user.    
    /// </summary>
    public async Task<string> Handle(CreateCheckoutSessionCommand command)
    {
        logger.LogInformation(
            "Creates a Checkout session for User {UserId} - Plan {PlanId}",
            command.UserId,
            command.PlanId);

        // 1. Validate that the user exists
        var userInfo = await externalIamService.GetUserInfoAsync(command.UserId);
        if (userInfo == null)
            throw new ArgumentException($"User {command.UserId} not found");

        // 2. Validate that the plan exists
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null)
            throw new ArgumentException($"Plan {command.PlanId} not found");

        if (string.IsNullOrEmpty(plan.StripePriceId))
            throw new InvalidOperationException($"Plan {plan.Name} does not have a Stripe Price ID configured");

        // 3. Verifies if the user already has an active subscription
        var existingSubscription = await subscriptionRepository.FindActiveByUserIdAsync(new UserId(command.UserId));

        if (existingSubscription != null)
            throw new InvalidOperationException(
                $"User {command.UserId} already has an active subscription");

        // 4. Creates a Customer in Stripe
        var stripeCustomerId = await paymentGateway.CreateOrGetCustomerAsync(
            command.UserId,
            userInfo.Email,
            userInfo.FullName);

        // 5. Creates a Checkout session
        var checkoutUrl = await paymentGateway.CreateCheckoutSessionAsync(
            stripeCustomerId,
            new StripePriceId(plan.StripePriceId),
            command.SuccessUrl,
            command.CancelUrl,
            command.TrialPeriodDays);

        logger.LogInformation(
            "Creates a Checkout session for a User {UserId}: {CheckoutUrl}",
            command.UserId,
            checkoutUrl);

        return checkoutUrl;
    }

    /// <summary>
    /// Cancels a subscription in Stripe.
    /// </summary>
    public async Task Handle(CancelSubscriptionInStripeCommand command)
    {
        logger.LogInformation(
            "Cancelling subscription {SubscriptionId} in Stripe (Immediately: {Immediately})",
            command.SubscriptionId,
            command.Immediately);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        var stripeSubscriptionId = new StripeSubscriptionId(subscription.StripeSubscriptionId);

        // 2. Cancels the subscription in Stripe
        if (command.Immediately)
        {
            await paymentGateway.CancelSubscriptionImmediatelyAsync(stripeSubscriptionId);
            subscription.UpdateStatus(ESubscriptionStatus.Cancelled);
        }
        else
        {
            var cancelAt = await paymentGateway.CancelSubscriptionAtPeriodEndAsync(stripeSubscriptionId);
            subscription.ScheduleCancellation(cancelAt);
        }

        // 3. Persist changes
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription {SubscriptionId} cancelled in Stripe",
            command.SubscriptionId);
    }

    /// <summary>
    /// Changes the plan of a subscription in Stripe.
    /// </summary>
    public async Task Handle(ChangeSubscriptionPlanInStripeCommand command)
    {
        logger.LogInformation(
            "Changing subscription plan {SubscriptionId} to Plan {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);

        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        // 2. Find the new plan
        var newPlan = await planRepository.FindByIdAsync(new PlanId(command.NewPlanId));
        if (newPlan == null)
            throw new ArgumentException($"Plan {command.NewPlanId} not found");

        if (string.IsNullOrEmpty(newPlan.StripePriceId))
            throw new InvalidOperationException($"Plan {newPlan.Name} does not have a Stripe Price ID");

        // 3. Updates in Stripe
        var stripeSubscriptionId = new StripeSubscriptionId(subscription.StripeSubscriptionId);
        var updatedStripeSubscription = await paymentGateway.UpdateSubscriptionPlanAsync(
            stripeSubscriptionId,
            new StripePriceId(newPlan.StripePriceId),
            command.ProrationBehavior);

        // 4. Update in our DB
        subscription.ChangePlan(
            new PlanId(command.NewPlanId),
            updatedStripeSubscription.CurrentPeriodEnd);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription plan {SubscriptionId} updated to {NewPlanId}",
            command.SubscriptionId,
            command.NewPlanId);
    }
}

using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

/// <summary>
/// Service to manage commands related to Stripe webhooks.
/// </summary>
public class StripeWebhookCommandService(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository, IUnitOfWork unitOfWork, ILogger<StripeWebhookCommandService> logger) : IStripeWebhookCommandService
{ 
    /// <summary>
    /// Sync a subscription from Stripe to our DB.
    /// </summary>
    public async Task Handle(SyncSubscriptionFromStripeCommand command)
    {
        logger.LogInformation(
            "Syncing subscription from Stripe: {StripeSubscriptionId}",
            command.StripeSubscriptionId);

        // 1. Search for existing subscription by StripeSubscriptionId
        var subscription = await subscriptionRepository
            .FindByStripeSubscriptionIdAsync(command.StripeSubscriptionId);

        if (subscription == null)
        {
            logger.LogWarning(
                "Subscription not found in DB for Stripe ID {StripeSubscriptionId}. " +
                "This can be normal if it was created directly in Stripe.",
                command.StripeSubscriptionId);

            // TODO: Optionally, create the subscription if it doesn't exist
            // This would require looking up the Customer by StripeCustomerId and associating it with a UserId
            return;
        }

        // 2. Map Stripe status to our domain status
        var newStatus = MapStripeStatusToSubscriptionStatus(command.Status);

        // 3. Update the subscription
        subscription.UpdateStatus(newStatus);
        subscription.UpdateEndDate(command.CurrentPeriodEnd);
        subscription.UpdateTrialEndsAt(command.TrialEnd);

        // 4. Handle cancellation
        if (command.CancelAtPeriodEnd && command.CancelAt.HasValue)
        {
            subscription.ScheduleCancellation(command.CancelAt.Value);
        }

        // 5. Persist changes
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription {SubscriptionId} synced: Status {Status}, EndDate {EndDate}",
            subscription.Id,
            newStatus,
            command.CurrentPeriodEnd);

        // 6. Domain events will be automatically published in UnitOfWork.CompleteAsync()
        // If we change the status, SubscriptionStatusChangedEvent will be published
    }

    private ESubscriptionStatus MapStripeStatusToSubscriptionStatus(string stripeStatus)
    {
        return stripeStatus.ToLowerInvariant() switch
        {
            "active" => ESubscriptionStatus.Active,
            "trialing" => ESubscriptionStatus.Trial,
            "past_due" => ESubscriptionStatus.PaymentDue,
            "canceled" => ESubscriptionStatus.Cancelled,
            "unpaid" => ESubscriptionStatus.Expired,
            "incomplete" => ESubscriptionStatus.Pending,
            "incomplete_expired" => ESubscriptionStatus.Expired,
            _ => ESubscriptionStatus.Pending
        };
    }
}

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork,
    ExternalProfileService externalProfileService,
    ExternalIamService externalIamService,
    IMediator mediator
) : ISubscriptionCommandService
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command)
    {
        // 1. Validate User existence
        if (!await externalIamService.UserExistsAsync(command.UserId))
            throw new ArgumentException($"User with ID {command.UserId} does not exist in IAM.");
        
        // 2. Validate plan existence
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null)
            throw new ArgumentException($"Plan with ID {command.PlanId} not found.");
        
        // 3. Validate Technician existence and retrieve their information
        if (plan.TargetRole == EUserRole.Technician)
        {
            var technicianInfo = await externalProfileService.FetchTechnicianInfoByProfileIdAsync(command.UserId);
            if (technicianInfo is null)
                throw new InvalidOperationException($"Technician not found for profile {command.UserId}");
        }

        // 4. Create new subscription aggregate
        var subscription = new Subscription(
            new UserId(command.UserId),
            new PlanId(command.PlanId),
            command.StartDate,
            command.EndDate,
            command.StripeCustomerId,
            command.StripeSubscriptionId,
            command.InitialStatus,
            command.TrialEndsAt
        );

        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        
        return subscription.Id.Value;
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(UpdateSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.UpdateStatus(command.NewStatus);
        subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., SubscriptionStatusChangedEvent)
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        
        return subscription.Id.Value; 
    }

    /// <inheritdoc/>
    public async Task Handle(CancelSubscriptionCommand command) // Void return
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) throw new ArgumentException($"Subscription with ID {command.SubscriptionId} not found.");

        subscription.ScheduleCancellation(command.CancellationEffectiveDate);
        await unitOfWork.CompleteAsync();

        // Publish domain event
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ActivateTrialCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.ActivateTrial(command.TrialEndDate);
        // subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event 
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();

        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(IncrementSubscriptionUsageCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.IncrementUsage();
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();
        
        
        // 📌 TODO: Publish (e.g., SubscriptionUsageIncrementedEvent) domain event if it is needed.
        // await mediator.Publish(new SubscriptionUsageIncrementedEvent(subscription.Id.Value, subscription.UsageCount, DateTime.UtcNow));
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ResetSubscriptionUsageCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        subscription.ResetUsage();
        // subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task Handle(ApplyDiscountCommand command) // Void return
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) throw new ArgumentException($"Subscription with ID {command.SubscriptionId} not found.");

        // TODO: Implement discount logic (e.g., interact with Stripe API to apply coupon)
        // This would likely involve updating a discount related field on the subscription or a Stripe call.
        // _subscriptionRepository.Update(subscription); // Changed to void if subscription state changes
        await unitOfWork.CompleteAsync();

        // 📌 TODO: Publish domain event (e.g., DiscountAppliedEvent) if it is needed to apply a discount.
        // await _mediator.Publish(new DiscountAppliedEvent(subscription.Id.Value, command.DiscountCode, DateTime.UtcNow));
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ChangeSubscriptionPlanCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription == null) return null;

        var oldPlanId = subscription.PlanId;
        var newPlanId = new PlanId(command.NewPlanId);
        var newPlan = await planRepository.FindByIdAsync(newPlanId);
        if (newPlan == null) 
            throw new ArgumentException($"New plan with ID {newPlanId} not found.");
        
        subscription.ChangePlan(newPlanId, command.NewEndDate);
        subscription.UpdateStripeSubscriptionId(command.StripeSubscriptionId); // Update if Stripe sub ID changes with plan
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., SubscriptionPlanChangedEvent)
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        
        return subscription.Id.Value; // Return the ID
    }
    
    
}

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class GetLocalSubscriptionIdQueryHandler 
    : IRequestHandler<GetLocalSubscriptionIdQuery, Guid?>
{
    private readonly ISubscriptionQueryService _queryService;

    public GetLocalSubscriptionIdQueryHandler(ISubscriptionQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<Guid?> Handle(GetLocalSubscriptionIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.Handle(query);
    }
}


using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

/// <summary>
/// <para>Implementation of <see cref="IPaymentTransactionQueryService"/>.</para>
/// </summary>
public class PaymentTransactionQueryService(IPaymentTransactionRepository paymentTransactionRepository) : IPaymentTransactionQueryService{

    /// <inheritdoc/>
    public async Task<IEnumerable<PaymentTransaction>> Handle(GetPaymentTransactionsBySubscriptionIdQuery query)
    {
        return await paymentTransactionRepository.ListBySubscriptionIdAsync(query.SubscriptionId);
    }
}   

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class PlanQueryService(IPlanRepository planRepository) : IPlanQueryService
{
    public async Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query)
    {
        return await planRepository.ListAsync();
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetDefaultPlanByRoleQuery query)
    {
        return await planRepository.FindDefaultPlanByRoleAsync(query.Role);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Plan>> Handle(GetPlansByRoleQuery query)
    {
        return await planRepository.ListPlansByRoleAsync(query.Role);
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetPlanByIdQuery query)
    {
        return await planRepository.FindByIdAsync(new PlanId(query.PlanId));
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetDefaultPlanQuery query)
    {
        return await planRepository.FindDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetPlanDetailsQuery query)
    {
        return await planRepository.FindByIdAsync(new PlanId(query.PlanId));
    }
}

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository) : ISubscriptionQueryService
{
    
    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListAsync();
    }

    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await subscriptionRepository.FindBySubscriptionIdAsync(new SubscriptionId(query.SubscriptionId));
    }

    public async Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query)
    {
        return await subscriptionRepository.FindByUserIdAsync(new UserId(query.UserId.Value));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Subscription>> Handle(GetAllActiveSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListActiveAsync();
    }

    /// <inheritdoc/>
    public async Task<Benefit?> Handle(GetUserBenefitQuery query)
    {
        var userId = new UserId(query.UserId);
        var subscription = await subscriptionRepository.FindByUserIdAsync(userId); // Corrected: int UserId
        if (subscription == null)
        {
            // If no explicit subscription, consider the default freemium plan (if any)
            // This assumes a user without an active subscription defaults to a freemium experience.
            var defaultFreemiumPlan = (await planRepository.ListPlansByRoleAsync(EUserRole.All)) // Query all for All or specific role
                .FirstOrDefault(p => p.MonetizationType == EMonetizationType.Free);

            if (defaultFreemiumPlan != null)
            {
                return defaultFreemiumPlan.GetBenefit(query.BenefitType);
            }
            return null;
        }

        var plan = await planRepository.FindByIdAsync(subscription.PlanId);
        if (plan == null) return null;

        return plan.GetBenefit(query.BenefitType);
    }

    public async Task<Guid?> Handle(GetLocalSubscriptionIdQuery query)
    {
        var subscription = await subscriptionRepository.FindByStripeSubscriptionIdAsync(query.StripeSubscriptionId);
        return subscription?.Id.Value; // Accedes al Guid del ValueObject
    }
}


using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="PaymentProcessedEvent"/> domain event.
/// </summary>
public class PaymentProcessedEventHandler : IEventHandler<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedEventHandler> _logger;

    public PaymentProcessedEventHandler(ILogger<PaymentProcessedEventHandler> logger)
    {
        _logger = logger;
        _logger.LogInformation("[PaymentProcessedEventHandler CTOR] Instanciando PaymentProcessedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: PaymentProcessedEvent recibido para Transacción: {notification.TransactionId}, Estado: {notification.Status}.");
        // Aquí podrías desencadenar notificaciones al usuario, actualizar registros de facturación, etc.
        await Task.CompletedTask;
    }
}

using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="PlanCreatedEvent"/> domain event.
/// </summary>
public class PlanCreatedEventHandler : IEventHandler<PlanCreatedEvent>
{
    private readonly ILogger<PlanCreatedEventHandler> _logger;

    public PlanCreatedEventHandler(ILogger<PlanCreatedEventHandler> logger)
    {
        _logger = logger;
        _logger.LogInformation("[PlanCreatedEventHandler CTOR] Instanciando PlanCreatedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(PlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: PlanCreatedEvent recibido para Plan: {notification.PlanId}, Nombre: {notification.Name}.");
        // Aquí podrías, por ejemplo, registrar el nuevo plan en un sistema de marketing o analíticas.
        await Task.CompletedTask;
    }
}

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

public class StripeEventHandler(IMediator mediator, IConfiguration configuration) 
    : IRequestHandler<ProcessStripeEventCommand>
{
    // Carga el secreto del webhook desde la configuración de forma segura.
    private readonly string _webhookSecret = configuration["Stripe:WebhookSecret"] 
                                            ?? throw new InvalidOperationException("Stripe Webhook Secret not configured.");

    public async Task<Unit> Handle(ProcessStripeEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Construye el evento de Stripe de forma segura, verificando la firma.
            var stripeEvent = EventUtility.ConstructEvent(
                request.StripeEventJson,
                request.StripeSignatureHeader,
                _webhookSecret
            );

            // Usa un 'switch' para manejar los diferentes tipos de eventos de Stripe.
            switch (stripeEvent.Type)
            {
                // Este evento se dispara cuando la sesión de checkout inicial se completa.
                case EventTypes.CheckoutSessionCompleted:
                    var session = stripeEvent.Data.Object as Session;
                    if (session?.Metadata == null || !session.Metadata.ContainsKey("local_subscription_id"))
                    {
                        Console.WriteLine("Checkout session completed event received but missing required metadata.");
                        return Unit.Value;
                    }

                    if (!Guid.TryParse(session.Metadata["local_subscription_id"], out var localSubscriptionIdGuid))
                    {
                        Console.WriteLine($"Invalid subscription ID format: {session.Metadata["local_subscription_id"]}");
                        return Unit.Value;
                    }    
                    if (session.PaymentIntentId == null) return Unit.Value;

                    var processInitialPaymentCommand = new ProcessPaymentCommand(
                        localSubscriptionIdGuid,
                        (decimal)session.AmountTotal.GetValueOrDefault() / 100,
                        session.Currency,
                        DateTime.UtcNow,
                        EPaymentStatus.Success,
                        session.PaymentIntentId,
                        "Pago de suscripción inicial a través de Checkout"  
                    );
                    
                    await mediator.Send(processInitialPaymentCommand, cancellationToken);
                    
                    Console.WriteLine($"Processed initial payment for subscription ID: {localSubscriptionIdGuid}");
                    break;

                // Este evento se dispara para los pagos recurrentes de la suscripción.
                case EventTypes.InvoicePaymentSucceeded:
                {
                    /*var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                    if (invoice == null)
                    {
                        Console.WriteLine("El objeto no es una factura válida");
                        return Unit.Value;
                    }

// Imprimir todas las propiedades disponibles para depuración
                    foreach (var prop in invoice.GetType().GetProperties())
                    {
                        try
                        {
                            var value = prop.GetValue(invoice);
                            Console.WriteLine($"Propiedad: {prop.Name}, Valor: {value}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al leer propiedad {prop.Name}: {ex.Message}");
                        }
                    }*/
                    var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                    if (invoice == null)
                    {
                        Console.WriteLine("El objeto no es una factura válida");
                        return Unit.Value;
                    }

                    var invoiceService = new InvoiceService();
                    invoice = invoiceService.Get(invoice.Id, new InvoiceGetOptions
                    {
                        Expand = new List<string> { "subscription" }
                    });

                    var subscriptionId = invoice.Lines?.Data?.FirstOrDefault()?.Subscription?.Id;

                    if (string.IsNullOrEmpty(subscriptionId))
                    {
                        Console.WriteLine("No subscription ID found in invoice.");
                        return Unit.Value;
                    }

                    // Ahora sí lo tienes garantizado
                    var getLocalIdQuery = new GetLocalSubscriptionIdQuery(subscriptionId);
                    var localSubscriptionIdResult = await mediator.Send(getLocalIdQuery, cancellationToken);

                    if (localSubscriptionIdResult is null)
                    {
                        Console.WriteLine($"No se encontró suscripción local para Stripe subscription: {subscriptionId}");
                        return Unit.Value;
                    }

                    var processRecurrentPaymentCommand = new ProcessPaymentCommand(
                        localSubscriptionIdResult.Value,
                        (decimal)invoice.AmountPaid / 100,
                        invoice.Currency,
                        DateTime.UtcNow,
                        EPaymentStatus.Success,
                        invoice.Id,
                        "Pago de suscripción recurrente"
                    );

                    await mediator.Send(processRecurrentPaymentCommand, cancellationToken);

                    Console.WriteLine($"Processed recurrent payment for subscription {subscriptionId}, invoice {invoice.Id}");
                    break;
                }

                
                // Si el evento no es uno de los anteriores, se ignora.
                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Error de Stripe en webhook: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocurrió un error inesperado al procesar el webhook: {e.Message}");
            throw;
        }

        return Unit.Value;
    }
}

using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionCreatedEvent"/> domain event.
/// </summary>
public class SubscriptionCreatedEventHandler : IEventHandler<SubscriptionCreatedEvent>
{
    private readonly ILogger<SubscriptionCreatedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionCreatedEventHandler(
        ILogger<SubscriptionCreatedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionCreatedEventHandler CTOR] Instanciando SubscriptionCreatedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionCreatedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}.");

        // Fetch the plan details to correctly populate the integration event
        var plan = await _planQueryService.Handle(new GetPlanByIdQuery(notification.PlanId.Value));
        if (plan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Plan with ID {notification.PlanId.Value} not found for SubscriptionCreatedEvent, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on plan benefits
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            notification.InitialStatus.ToString(),
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}

using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionPlanChangedEvent"/> domain event.
/// </summary>
public class SubscriptionPlanChangedEventHandler : IEventHandler<SubscriptionPlanChangedEvent>
{
    private readonly ILogger<SubscriptionPlanChangedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ISubscriptionQueryService _subscriptionQueryService; // To get subscription for current plan
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionPlanChangedEventHandler(
        ILogger<SubscriptionPlanChangedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        ISubscriptionQueryService subscriptionQueryService,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _subscriptionQueryService = subscriptionQueryService;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionPlanChangedEventHandler CTOR] Instanciando SubscriptionPlanChangedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionPlanChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionPlanChangedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}, Plan Anterior: {notification.OldPlanId.Value}, Nuevo Plan: {notification.NewPlanId.Value}.");

        // Fetch the subscription and new plan to determine detailed status for integration event
        var subscription = await _subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(notification.SubscriptionId));
        if (subscription == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Subscription with ID {notification.SubscriptionId} not found for plan changed event, cannot publish detailed integration event.");
            return;
        }

        var newPlan = await _planQueryService.Handle(new GetPlanByIdQuery(notification.NewPlanId.Value));
        if (newPlan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] New Plan with ID {notification.NewPlanId.Value} not found for Subscription {notification.SubscriptionId}, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on the new plan and subscription state
        bool isPremium = newPlan.MonetizationType != EMonetizationType.Free;
        bool isCertified = newPlan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = newPlan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            subscription.Status.ToString(), // Use current subscription status
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}

using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="SubscriptionStatusChangedEvent"/> domain event.
/// </summary>
public class SubscriptionStatusChangedEventHandler : IEventHandler<SubscriptionStatusChangedEvent>
{
    private readonly ILogger<SubscriptionStatusChangedEventHandler> _logger;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ISubscriptionQueryService _subscriptionQueryService; // To get subscription for current plan
    private readonly IPlanQueryService _planQueryService; // To get plan details for integration event

    public SubscriptionStatusChangedEventHandler(
        ILogger<SubscriptionStatusChangedEventHandler> logger,
        IIntegrationEventPublisher integrationEventPublisher,
        ISubscriptionQueryService subscriptionQueryService,
        IPlanQueryService planQueryService)
    {
        _logger = logger;
        _integrationEventPublisher = integrationEventPublisher;
        _subscriptionQueryService = subscriptionQueryService;
        _planQueryService = planQueryService;
        _logger.LogInformation("[SubscriptionStatusChangedEventHandler CTOR] Instanciando SubscriptionStatusChangedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(SubscriptionStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: SubscriptionStatusChangedEvent recibido para Suscripción: {notification.SubscriptionId}, Usuario: {notification.UserId.Value}, Nuevo Estado: {notification.NewStatus}.");

        // Fetch the subscription and plan to determine detailed status for integration event
        var subscription = await _subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(notification.SubscriptionId));
        if (subscription == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Subscription with ID {notification.SubscriptionId} not found for status changed event, cannot publish detailed integration event.");
            return;
        }

        var plan = await _planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));
        if (plan == null)
        {
            _logger.LogWarning($"[Subscriptions BC] Plan with ID {subscription.PlanId.Value} not found for Subscription {notification.SubscriptionId}, cannot publish detailed integration event.");
            return;
        }

        // Determine derived properties based on the actual plan and subscription state
        bool isPremium = plan.MonetizationType != EMonetizationType.Free;
        bool isCertified = plan.Benefits.Exists(b => b.Type == "CertificationAccess" && b.FlagValue == true);
        bool canUseBoost = plan.Benefits.Exists(b => b.Type == "BoostAccess" && b.FlagValue == true);

        var integrationEvent = new UserSubscriptionStatusChangedIntegrationEvent(
            notification.UserId.Value,
            notification.NewStatus.ToString(),
            isPremium,
            isCertified,
            canUseBoost,
            DateTime.UtcNow // Use current UTC time for the integration event
        );
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

        _logger.LogInformation($"[Subscriptions BC] Publicado Integration Event: UserSubscriptionStatusChangedIntegrationEvent para Usuario: {notification.UserId.Value}.");
    }
}
```

### 7. Model Building Extensions

```
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
    /// Applies the Entity Framework Core configuration for the Subscription and Payments Bounded Context.    /// </summary>    /// <param name="builder">The <see cref="ModelBuilder"/> instance.</param>    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)  
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
            subscriptionConfiguration.OwnsOne(s => s.UserId, userIdBuilder =>  
            {  
                userIdBuilder.Property(u => u.Value)  
                    .HasColumnName("UserId")  
                    .IsRequired();  
            });  
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
            subscriptionConfiguration.Property(s => s.StripeCustomerId).IsRequired();  
            subscriptionConfiguration.Property(s => s.StripeSubscriptionId).IsRequired();  
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
            planConfiguration.Property(p => p.StripePriceId).HasMaxLength(100);  
  
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
    }  
}
```
