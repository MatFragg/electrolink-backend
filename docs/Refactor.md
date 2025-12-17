# Plan de Refactor: Subscriptions BC - N-Tier Layer DDD Clean Architecture

## Contexto de la Implementación Actual

### Arquitectura Confirmada
- **Estilo**: N-Tier Layer DDD Clean Architecture (NO Hexagonal)
- **Patrones**: CQRS (sin Mediator), Assembler (Resources/Transform), Domain Events + Integration Events
- **Commands**: Ubicados en `Domain/Model/Commands` (intencional para usar Value Objects y sobrecarga)
- **Eventos**: Domain Events (deprecando Integration Events para monolito)

### Estructura Actual

```
Subscriptions/
├── Domain/
│   ├── Services/
│   │   └── IPaymentGatewayService.cs          # Abstracción de dominio
│   ├── Repository/
│   │   └── I*Repository.cs
│   └── Model/
│       ├── Commands/                            # ✅ Intencional en Domain
│       ├── Queries/
│       ├── Events/
│       │   ├── Domain/                          # Para bounded context
│       │   └── Integration/                     # ⚠️ Deprecar para monolito
│       ├── Aggregates/
│       ├── Entities/
│       └── ValueObjects/
├── Application/
│   └── Internal/
│       ├── CommandServices/
│       ├── QueryServices/
│       ├── EventHandlers/
│       └── OutboundServices/
│           ├── IStripeService.cs                # ⚠️ PROBLEMA: Duplicación
│           ├── ExternalIamService.cs
│           └── ExternalProfileService.cs
├── Infrastructure/
│   ├── PaymentGateway/
│   │   └── Stripe/
│   │       ├── StripePaymentGatewayService.cs  # Implementa IPaymentGatewayService
│   │       ├── StripeClientFactory.cs
│   │       ├── StripeEventMapper.cs
│   │       ├── StripeSettings.cs
│   │       └── Webhooks/
│   │           ├── StripeWebhookEventProcessor.cs
│   │           └── StripeWebhookValidator.cs
│   └── Persistence/EFC/
│       ├── Repositories/
│       ├── Configuration/
│       └── Configuration/Extensions/
└── Interfaces/
    ├── ACL/                                     # Anti-Corruption Layer
    └── REST/
        ├── Controllers/
        ├── Resources/
        └── Transform/                            # Assembler pattern
```

---

## Análisis de Problemas Arquitectónicos Identificados

### 🔴 CRÍTICO 1: Duplicación de Abstracciones de Payment Gateway

**Problema:**
Tienes DOS interfaces para el mismo concepto:

1. `Domain/Services/IPaymentGatewayService.cs` - Abstracción de dominio
2. `Application/Internal/OutboundServices/IStripeService.cs` - Abstracción específica de Stripe

**Por qué es un problema:**
- Viola DRY (Don't Repeat Yourself)
- Genera confusión sobre cuál usar
- `IStripeService` en Application está acoplado a Stripe (viola Clean Architecture)
- Duplicación de responsabilidades

**Impacto:**
- 🔴 Alto - Rompe principios fundamentales de Clean Architecture

**Solución Recomendada:**

```
ELIMINAR: Application/Internal/OutboundServices/IStripeService.cs
MANTENER: Domain/Services/IPaymentGatewayService.cs

RENOMBRAR:
Domain/Services/IPaymentGatewayService.cs 
    → Domain/Services/IPaymentGatewayService.cs (mantener)

IMPLEMENTACIÓN:
Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs 
    → Debe implementar Domain/Services/IPaymentGatewayService
```

**Justificación:**
- En DDD, las abstracciones de servicios externos van en `Domain/Services`
- El dominio define QUÉ necesita, Infrastructure define CÓMO lo hace
- El nombre `IStripeService` rompe la abstracción (¿qué pasa si cambias a PayPal?)

---

### 🔴 CRÍTICO 2: Nombre Acoplado a Implementación

**Problema:**
`IStripeService` tiene el nombre del proveedor en la interfaz

**Por qué es un problema:**
- Viola Dependency Inversion Principle
- Si cambias de Stripe a MercadoPago/PayPal, el nombre no tiene sentido
- Las abstracciones deben ser agnósticas al proveedor

**Solución:**

```csharp
// ❌ MAL - Acoplado a Stripe
public interface IStripeService
{
    Task<string> CreateStripeCustomer(...);
    Task<string> CreateStripeSubscription(...);
}

// ✅ BIEN - Agnóstico
namespace ElectroLink.Subscriptions.Domain.Services;

public interface IPaymentGatewayService
{
    // Customer Management
    Task<PaymentCustomer> CreateCustomerAsync(
        UserId userId,
        EmailAddress email,
        FullName name,
        CancellationToken cancellationToken = default);
    
    // Subscription Management
    Task<PaymentSubscription> CreateSubscriptionAsync(
        PaymentCustomerId customerId,
        PlanId planId,
        CancellationToken cancellationToken = default);
    
    Task CancelSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        CancellationToken cancellationToken = default);
    
    Task<PaymentSubscription> GetSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        CancellationToken cancellationToken = default);
    
    // Checkout Session
    Task<CheckoutSession> CreateCheckoutSessionAsync(
        CheckoutSessionRequest request,
        CancellationToken cancellationToken = default);
}

// Value Objects para abstraer IDs de pasarela
public record PaymentCustomerId(string Value);
public record PaymentSubscriptionId(string Value);
public record PaymentCustomer(PaymentCustomerId Id, string Email, string Name);
public record PaymentSubscription(
    PaymentSubscriptionId Id,
    PaymentSubscriptionStatus Status,
    DateTime CurrentPeriodEnd,
    DateTime? CanceledAt);

public enum PaymentSubscriptionStatus
{
    Active,
    Canceled,
    PastDue,
    Trialing,
    Incomplete,
    Paused
}

public record CheckoutSession(
    string SessionId,
    string Url,
    DateTime ExpiresAt
);
```

---

### 🟡 MEDIO 1: Commands en Domain con Lógica de Infraestructura

**Problema Detectado:**
Tienes commands como:
- `CancelSubscriptionInGatewayCommand`
- `ChangeSubscriptionPlanInGatewayCommand`
- `SyncSubscriptionFromGatewayCommand`

**Análisis:**
Mencionas que Commands están en Domain para:
1. Usar Value Objects como parámetros ✅ Válido
2. Sobrecarga de constructores en Aggregates ✅ Válido
3. Sobrecarga en contratos de Domain/Services ✅ Válido

**PERO**, commands con "InGateway" o "FromGateway" tienen concerns de Infrastructure.

**Recomendación:**

```
OPCIÓN A (Recomendada): Command sin detalles de implementación

// ❌ Actual - Menciona "Gateway" (detalle de infra)
Domain/Model/Commands/CancelSubscriptionInGatewayCommand.cs

// ✅ Propuesto - Agnóstico
Domain/Model/Commands/CancelSubscriptionCommand.cs

El CommandService decide si necesita llamar al gateway o no
```

**Ejemplo:**

```csharp
// Domain/Model/Commands/CancelSubscriptionCommand.cs
namespace ElectroLink.Subscriptions.Domain.Model.Commands;

public record CancelSubscriptionCommand(
    SubscriptionId SubscriptionId,
    CancellationReason Reason,
    bool CancelImmediately = false
);

// Application/Internal/CommandServices/SubscriptionCommandService.cs
public class SubscriptionCommandService
{
    private readonly ISubscriptionRepository _repository;
    private readonly IPaymentGateway Service _paymentGateway; // ← Domain/Services

    public async Task HandleAsync(CancelSubscriptionCommand command)
    {
        var subscription = await _repository.FindByIdAsync(command.SubscriptionId);
        
        // 1. Cancelar en gateway (si tiene payment subscription)
        if (subscription.PaymentSubscriptionId != null)
        {
            await _paymentGatewayService.CancelSubscriptionAsync(
                subscription.PaymentSubscriptionId,
                command.CancellationToken
            );
        }
        
        // 2. Cancelar en dominio
        subscription.Cancel(command.Reason);
        
        await _repository.SaveChangesAsync();
    }
}
```

---

### 🟡 MEDIO 2: Integration Events en Monolito

**Problema:**
Tienes `Domain/Model/Events/Integration/` pero mencionas que está en desuso.

**Recomendación:**

```
FASE 1 (Inmediata): Marcar como Obsolete

// Integration Events existentes
[Obsolete("Integration Events are deprecated for monolith. Use Domain Events instead.")]
public record PaymentProcessedIntegrationEvent(...)

FASE 2 (Gradual): Migrar a Domain Events

// Eliminar carpeta Integration/
// Mover eventos relevantes a Domain/ si aplica
// O usar eventos de dominio normales

FASE 3 (Limpieza): Eliminar completamente

// Remover:
- Domain/Model/Events/Integration/
- Application/Internal/EventHandlers/*IntegrationEventPublisher.cs
```

**Justificación:**
- En monolitos, los Domain Events son suficientes
- Integration Events son para comunicación entre microservicios
- Simplifica la arquitectura

---

### 🟢 BAJO 1: Múltiples Controllers para Stripe

**Observación:**
Tienes controllers separados:
- `StripeController.cs`
- `StripeWebhooksController.cs`
- `CheckoutController.cs`

**Análisis:**
Esto puede ser correcto SI:
- `StripeWebhooksController` maneja solo webhooks (correcto)
- `CheckoutController` maneja flujo de checkout INDEPENDIENTE de Stripe
- `StripeController` es para operaciones administrativas de Stripe?

**Pregunta para validar:**
¿`StripeController` expone operaciones específicas de Stripe o debería ser parte de `SubscriptionsController`?

**Recomendación (SI expone detalles de Stripe):**

```
❌ NO exponer detalles de Stripe en la API pública

// Evitar endpoints como:
GET /api/stripe/customers/{id}        # ← Stripe es detalle de implementación
POST /api/stripe/sync-subscription    # ← Stripe es detalle de implementación

✅ Exponer operaciones de negocio

// Mejor:
GET /api/subscriptions/{id}
POST /api/subscriptions/{id}/sync      # ← No menciona Stripe
```

---

### 🟢 BAJO 2: Naming de Assemblers

**Observación:**
Usas sufijos:
- `CommandFromResourceAssembler`
- `ResourceFromEntityAssembler`

**Recomendación (Opcional - Estilo):**

Considerar nombres más concisos:

```csharp
// Actual (verboso pero explícito)
public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommand(this CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(...);
    }
}

// Alternativa (más conciso)
public static class SubscriptionResourceMapper
{
    // Resource → Command
    public static CreateSubscriptionCommand ToCommand(this CreateSubscriptionResource resource) 
        => new(...);
    
    // Entity → Resource
    public static SubscriptionResource ToResource(this Subscription subscription) 
        => new(...);
}
```

**Decisión:** Esto es preferencia de estilo. Si tu equipo prefiere `*FromResourceAssembler`, está bien.

---

## Plan de Refactor Priorizado

### FASE 1: Unificar Abstracciones de Payment Gateway (CRÍTICO) - 1 día

| Paso | Acción | Archivos Afectados |
|------|--------|-------------------|
| 1.1 | Revisar `IPaymentGatewayService` en Domain/Services | `Domain/Services/IPaymentGatewayService.cs` |
| 1.2 | Comparar con `IStripeService` y consolidar métodos | `Application/Internal/OutboundServices/IStripeService.cs` |
| 1.3 | Asegurar que `StripePaymentGatewayService` implementa `IPaymentGatewayService` | `Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs` |
| 1.4 | Actualizar CommandServices para usar `IPaymentGatewayService` (Domain) en lugar de `IStripeService` (Application) | `Application/Internal/CommandServices/*.cs` |
| 1.5 | Eliminar `IStripeService.cs` | `Application/Internal/OutboundServices/IStripeService.cs` |
| 1.6 | Actualizar Dependency Injection | `Program.cs` o `Startup.cs` |

**Ejemplo de Cambio:**

```csharp
// ANTES - Incorrecto
namespace ElectroLink.Subscriptions.Application.Internal.CommandServices;

using ElectroLink.Subscriptions.Application.Internal.OutboundServices; // ← Mal

public class SubscriptionCommandService
{
    private readonly IStripeService _stripeService; // ← Acoplado a Stripe
    
    public SubscriptionCommandService(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }
}

// DESPUÉS - Correcto
namespace ElectroLink.Subscriptions.Application.Internal.CommandServices;

using ElectroLink.Subscriptions.Domain.Services; // ← Correcto

public class SubscriptionCommandService
{
    private readonly IPaymentGatewayService _paymentGateway; // ← Agnóstico
    
    public SubscriptionCommandService(IPaymentGatewayService paymentGateway)
    {
        _paymentGateway = paymentGateway;
    }
}
```

---

### FASE 2: Refactor de Commands con Detalles de Infraestructura (MEDIO) - 1 día

| Paso | Acción | Archivos Afectados |
|------|--------|-------------------|
| 2.1 | Identificar Commands con "InGateway", "FromGateway" | `Domain/Model/Commands/*InGateway*.cs` |
| 2.2 | Renombrar a nombres agnósticos | `CancelSubscriptionCommand.cs`, etc. |
| 2.3 | Actualizar CommandServices para manejar lógica de gateway internamente | `Application/Internal/CommandServices/*.cs` |
| 2.4 | Verificar que Commands solo contienen datos de dominio, no detalles de implementación | Todos los Commands |

**Commands a Revisar:**

```
✅ BIEN (nombres agnósticos):
- CreateSubscriptionCommand
- CancelSubscriptionCommand
- ChangeSubscriptionPlanCommand
- ActivateTrialCommand
- ApplyDiscountCommand

⚠️ REVISAR (posible coupling a infra):
- CancelSubscriptionInGatewayCommand      → CancelSubscriptionCommand?
- ChangeSubscriptionPlanInGatewayCommand  → ChangeSubscriptionPlanCommand?
- SyncSubscriptionFromGatewayCommand      → SyncSubscriptionCommand?
```

**Decisión por Command:**

Si el command SOLO hace sentido en contexto de gateway (ej: `SyncSubscriptionFromGatewayCommand`), considera:
1. ¿Es realmente un command de dominio o es una operación de infraestructura?
2. Podría ser un método privado del CommandService en lugar de un command público?

---

### FASE 3: Deprecar y Migrar Integration Events (MEDIO) - 1 día

| Paso | Acción | Archivos Afectados |
|------|--------|-------------------|
| 3.1 | Marcar Integration Events como `[Obsolete]` | `Domain/Model/Events/Integration/*.cs` |
| 3.2 | Marcar Integration Event Publishers como `[Obsolete]` | `Application/Internal/EventHandlers/*IntegrationEventPublisher.cs` |
| 3.3 | Documentar plan de migración a Domain Events | Documentación |
| 3.4 | (Gradual) Reemplazar uso de Integration Events con Domain Events | Varios |
| 3.5 | (Futuro) Eliminar carpeta Integration/ y publishers | Limpieza final |

---

### FASE 4: Revisión de Separación de Concerns en Controllers (BAJO) - 0.5 días

| Paso | Acción | Archivos Afectados |
|------|--------|-------------------|
| 4.1 | Revisar `StripeController.cs` - ¿Expone detalles de Stripe? | `Interfaces/REST/StripeController.cs` |
| 4.2 | Si expone detalles de Stripe, mover endpoints a `SubscriptionsController` con nombres agnósticos | `Interfaces/REST/SubscriptionsController.cs` |
| 4.3 | `StripeWebhooksController` está correcto (maneja webhooks) | `Interfaces/REST/StripeWebhooksController.cs` |
| 4.4 | `CheckoutController` - Validar que no expone detalles de Stripe | `Interfaces/REST/CheckoutController.cs` |

---

### FASE 5: Mejoras Opcionales de Calidad (BAJO) - Continuo

| Categoría | Acción |
|-----------|--------|
| **Logging** | Agregar logging estructurado en CommandServices, PaymentGatewayService, Webhooks |
| **Validación** | Validar Commands antes de ejecutar (FluentValidation?) |
| **Resilience** | Agregar Polly para retry en llamadas a Stripe |
| **Observability** | Métricas de pagos exitosos/fallidos, latencia de Stripe |
| **Testing** | Unit tests para CommandServices (mockear IPaymentGatewayService) |
| **Testing** | Integration tests para StripePaymentGatewayService (Stripe Test Mode) |

---

## Checklist de Validación Post-Refactor

### ✅ Principios DDD

- [ ] Domain Layer no tiene dependencias de Infrastructure
- [ ] Domain/Services contiene abstracciones de servicios externos
- [ ] Application depende de Domain, no de Infrastructure directamente
- [ ] Commands en Domain solo contienen datos de dominio (sin detalles de Stripe)
- [ ] Aggregates implementan lógica de negocio

### ✅ Clean Architecture

- [ ] Dependencias apuntan hacia adentro (Infrastructure → Application → Domain)
- [ ] No hay referencias cruzadas entre capas del mismo nivel
- [ ] Abstracciones definidas en capas internas, implementaciones en externas
- [ ] Domain es puro (sin EF Core, sin Stripe SDK)

### ✅ Separación de Concerns

- [ ] Application no conoce detalles de Stripe (usa IPaymentGatewayService de Domain)
- [ ] Infrastructure implementa detalles técnicos
- [ ] Controllers no exponen detalles de implementación (Stripe) en URLs/responses
- [ ] Assemblers convierten entre capas apropiadamente

### ✅ Testabilidad

- [ ] CommandServices se pueden testear mockeando IPaymentGatewayService
- [ ] No se requiere Stripe real para unit tests de Application
- [ ] Integration tests usan Stripe Test Mode
- [ ] Webhook handlers tienen tests con eventos simulados

---

## Preguntas para Validar Implementación Actual

Antes de proceder con refactor, necesito confirmar:

### 1. Domain/Services/IPaymentGatewayService.cs

**Pregunta:** ¿Qué métodos tiene actualmente esta interfaz?

**Escenarios posibles:**
- **A)** Tiene métodos genéricos similares a los que propongo arriba → Perfecto
- **B)** Está vacía o tiene pocos métodos → Necesita expandirse
- **C)** Tiene métodos muy específicos de Stripe → Necesita refactor

### 2. Application/Internal/OutboundServices/IStripeService.cs

**Pregunta:** ¿Qué relación tiene con `IPaymentGatewayService`?

**Escenarios:**
- **A)** Son interfaces completamente diferentes → Consolidar
- **B)** `IStripeService` hereda de `IPaymentGatewayService` → Eliminar herencia innecesaria
- **C)** `IStripeService` tiene métodos adicionales específicos de Stripe → Evaluar si pertenecen al dominio

### 3. Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs

**Pregunta:** ¿Qué interfaz(es) implementa actualmente?

**Escenarios:**
- **A)** Solo `IStripeService` (Application) → Cambiar a `IPaymentGatewayService` (Domain)
- **B)** Solo `IPaymentGatewayService` (Domain) → Perfecto, eliminar `IStripeService`
- **C)** Ambas → Eliminar `IStripeService`

### 4. Commands "InGateway"

**Pregunta:** ¿Por qué necesitas distinguir entre operaciones "en gateway" vs "en dominio"?

**Posibles razones:**
- **A)** Para separar responsabilidades → Considerar eliminar distinción, el CommandService maneja ambos
- **B)** Para operaciones que SOLO existen en gateway → Verificar si son realmente Commands de dominio

---

## Recomendaciones Específicas por Archivo

### Domain/Services/IPaymentGatewayService.cs

**Estado Ideal:**

```csharp
namespace ElectroLink.Subscriptions.Domain.Services;

/// <summary>
/// Puerto (abstracción) para interactuar con pasarelas de pago externas.
/// Esta interfaz pertenece al Domain Layer y define QUÉ necesita el dominio
/// de una pasarela de pagos, sin importar la implementación específica.
/// </summary>
public interface IPaymentGatewayService
{
    // === Customer Management ===
    
    Task<PaymentCustomer> CreateCustomerAsync(
        EmailAddress email,
        FullName name,
        CancellationToken cancellationToken = default);
    
    Task<PaymentCustomer> GetCustomerAsync(
        PaymentCustomerId customerId,
        CancellationToken cancellationToken = default);
    
    Task DeleteCustomerAsync(
        PaymentCustomerId customerId,
        CancellationToken cancellationToken = default);
    
    // === Subscription Management ===
    
    Task<PaymentSubscription> CreateSubscriptionAsync(
        PaymentCustomerId customerId,
        PlanId planId,
        CancellationToken cancellationToken = default);
    
    Task<PaymentSubscription> GetSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        CancellationToken cancellationToken = default);
    
    Task<PaymentSubscription> UpdateSubscriptionPlanAsync(
        PaymentSubscriptionId subscriptionId,
        PlanId newPlanId,
        CancellationToken cancellationToken = default);
    
    Task CancelSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        bool cancelImmediately = false,
        CancellationToken cancellationToken = default);
    
    Task<PaymentSubscription> PauseSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        CancellationToken cancellationToken = default);
    
    Task<PaymentSubscription> ResumeSubscriptionAsync(
        PaymentSubscriptionId subscriptionId,
        CancellationToken cancellationToken = default);
    
    // === Checkout ===
    
    Task<CheckoutSession> CreateCheckoutSessionAsync(
        CheckoutSessionRequest request,
        CancellationToken cancellationToken = default);
    
    // === Plan Management (si aplica) ===
    
    Task<PaymentPlan> CreatePlanAsync(
        PlanCreationRequest request,
        CancellationToken cancellationToken = default);
    
    Task<PaymentPlan> UpdatePlanAsync(
        string gatewayPlanId,
        PlanUpdateRequest request,  
        CancellationToken cancellationToken = default);
}

// Value Objects que abstraen IDs de pasarela
public record PaymentCustomerId(string Value);
public record PaymentSubscriptionId(string Value);
public record PaymentCustomer(
    PaymentCustomerId Id,
    string Email,
    string Name,
    DateTime CreatedAt
);

public record PaymentSubscription(
    PaymentSubscriptionId Id,
    PaymentSubscriptionStatus Status,
    PlanId PlanId,
    DateTime CurrentPeriodStart,
    DateTime CurrentPeriodEnd,
    DateTime? CanceledAt,
    DateTime? PausedAt
);

public enum PaymentSubscriptionStatus
{
    Active,
    Canceled,
    Incomplete,
    IncompleteExpired,
    Trialing,
    PastDue,
    Unpaid,
    Paused
}

public record CheckoutSession(
    string SessionId,
    string Url,
    DateTime ExpiresAt
);

public record CheckoutSessionRequest(
    EmailAddress CustomerEmail,
    PlanId PlanId,
    string SuccessUrl,
    string CancelUrl,
    Dictionary<string, string>? Metadata = null
);
```

---

### Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs

**Debe implementar:**

```csharp
namespace ElectroLink.Subscriptions.Infrastructure.PaymentGateway.Stripe;

using Stripe;
using ElectroLink.Subscriptions.Domain.Services; // ← Importante

public class StripePaymentGatewayService : IPaymentGatewayService
{
    private readonly IStripeClientFactory _clientFactory;
    private readonly ILogger<StripePaymentGatewayService> _logger;
    private readonly StripeSettings _settings;

    public StripePaymentGatewayService(
        IStripeClientFactory clientFactory,
        IOptions<StripeSettings> settings,
        ILogger<StripePaymentGatewayService> logger)
    {
        _clientFactory = clientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<PaymentCustomer> CreateCustomerAsync(
        EmailAddress email,
        FullName name,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var customerService = _clientFactory.CreateCustomerService();
            
            var options = new CustomerCreateOptions
            {
                Email = email.Value,
                Name = name.Value,
                Metadata = new Dictionary<string, string>
                {
                    { "Source", "ElectroLink" }
                }
            };

            var stripeCustomer = await customerService.CreateAsync(
                options,
                cancellationToken: cancellationToken
            );

            _logger.LogInformation(
                "Stripe customer created: {CustomerId} for {Email}",
                stripeCustomer.Id, email.Value
            );

            // Traducir de Stripe → Domain
            return new PaymentCustomer(
                Id: new PaymentCustomerId(stripeCustomer.Id),
                Email: stripeCustomer.Email,
                Name: stripeCustomer.Name,
                CreatedAt: stripeCustomer.Created
            );
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error creating customer for {Email}", email.Value);
            throw new PaymentGatewayException("Failed to create customer in payment gateway", ex);
        }
    }

    // ... implementar resto de métodos
}

// Exception propia
public class PaymentGatewayException : Exception
{
    public PaymentGatewayException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
```

---

### Application/Internal/CommandServices/SubscriptionCommandService.cs

**Debe usar `IPaymentGatewayService` de Domain:**

```csharp
namespace ElectroLink.Subscriptions.Application.Internal.CommandServices;

using ElectroLink.Subscriptions.Domain.Services; // ← Domain, no Application
using ElectroLink.Subscriptions.Domain.Model.Commands;
using ElectroLink.Subscriptions.Domain.Repository;

public class SubscriptionCommandService
{
    private readonly ISubscriptionRepository _repository;
    private readonly IPaymentGatewayService _paymentGateway; // ← De Domain
    private readonly ILogger<SubscriptionCommandService> _logger;

    public SubscriptionCommandService(
        ISubscriptionRepository repository,
        IPaymentGatewayService paymentGateway,
        ILogger<SubscriptionCommandService> logger)
    {
        _repository = repository;
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<Subscription> HandleAsync(
        CreateSubscriptionCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating subscription for user {UserId}", command.UserId);

        // 1. Validar reglas de negocio
        var existing = await _repository.FindActiveByUserIdAsync(command.UserId);
        if (existing != null)
        {
            throw new BusinessException("User already has an active subscription");
        }

        // 2. Crear customer en gateway
        var paymentCustomer = await _paymentGateway.CreateCustomerAsync(
            command.Email,
            command.CustomerName,
            cancellationToken
        );

        // 3. Crear subscription en gateway
        var paymentSubscription = await _paymentGateway.CreateSubscriptionAsync(
            paymentCustomer.Id,
            command.PlanId,
            cancellationToken
        );

        // 4. Crear aggregate de dominio
        var subscription = Subscription.Create( // Factory method
            userId: command.UserId,
            planId: command.PlanId,
            paymentCustomerId: paymentCustomer.Id,
            paymentSubscriptionId: paymentSubscription.Id
        );

        // 5. Persistir
        await _repository.AddAsync(subscription);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Subscription {SubscriptionId} created for user {UserId}",
            subscription.Id, command.UserId
        );

        return subscription;
    }
}
```

---

## Métricas de Éxito del Refactor

Después del refactor, deberías poder responder SÍ a todas:

- [ ] ¿Puedo cambiar de Stripe a PayPal sin modificar Application Layer?
- [ ] ¿`Domain/Services` contiene la única abstracción de payment gateway?
- [ ] ¿`IStripeService` ha sido eliminado completamente?
- [ ] ¿Ningún Command menciona detalles de infraestructura en su nombre?
- [ ] ¿Integration Events están marcados como obsoletos?
- [ ] ¿Controllers no exponen "Stripe" en sus endpoints públicos?
- [ ] ¿Todos los CommandServices usan `IPaymentGatewayService` de Domain?
- [ ] ¿Los tests de Application pueden ejecutarse sin Stripe real?

---

## Próximos Pasos

1. **Responde las preguntas de validación** de la sección anterior
2. **Revisa este plan** y confirma que se alinea con tu visión
3. **Prioriza las fases** según urgencia de negocio
4. **Implementa FASE 1** primero (unificar abstracciones) - mayor impacto
5. **Valida con tests** después de cada fase
6. **Documenta decisiones** arquitectónicas para el equipo

---

## Tiempo Estimado

| Fase | Esfuerzo | Riesgo |
|------|----------|--------|
| FASE 1: Unificar abstracciones | 1 día | 🔴 Alto si hay mucho acoplamiento |
| FASE 2: Refactor commands | 1 día | 🟡 Medio |
| FASE 3: Deprecar Integration Events | 1 día | 🟢 Bajo |
| FASE 4: Revisar controllers | 0.5 días | 🟢 Bajo |
| FASE 5: Mejoras opcionales | Continuo | 🟢 Bajo |

**Total:** 3.5 - 4 días de desarrollo activo

---

## Conclusión

Tu implementación ya tiene una estructura sólida con N-Tier DDD Clean Architecture. Los principales problemas son:

1. **Duplicación de abstracciones** (`IStripeService` vs `IPaymentGatewayService`)
2. **Naming acoplado a implementación** (Commands "InGateway")
3. **Integration Events innecesarios** en monolito

El refactor propuesto mantiene tus decisiones de diseño válidas (Commands en Domain, Assembler pattern) mientras corrige los problemas identificados siguiendo principios DDD y Clean Architecture.
