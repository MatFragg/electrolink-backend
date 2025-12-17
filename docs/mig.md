Plan de Migración: CreateCheckoutSession a IPaymentGatewayService
Contexto del Problema
Situación Actual:

✅ IStripeService (Application/Internal/OutboundServices) tiene método CreateCheckoutSession
❌ IPaymentGatewayService (Domain/Services) NO tiene este método
❌ StripeController tiene un endpoint que usa este método específico de Stripe
Pregunta Clave: ¿El concepto de "Checkout Session" es específico de Stripe o es un concepto de dominio?

Respuesta: ✅ ES UN CONCEPTO DE DOMINIO

Un "Checkout Session" es una abstracción de dominio que representa:

Un flujo de pago iniciado por el usuario
Una URL temporal donde el usuario completa el pago
Una experiencia de checkout independiente del proveedor
Por lo tanto: Debe estar en IPaymentGatewayService en Domain, pero abstrayendo los detalles de Stripe.

Estrategia de Migración
Opción Elegida: Abstracción de Checkout Session en Domain
Ventajas:

✅ Otros gateways (PayPal, MercadoPago) también tienen conceptos similares
✅ El dominio necesita "crear una sesión de pago externa" sin saber cómo
✅ Facilita testing sin depender de Stripe
✅ Permite cambiar de proveedor sin modificar Application/Controllers
Plan de Implementación (5 Pasos)
Paso 1: Agregar Abstracción de Checkout Session en Domain
Archivo: Domain/Services/IPaymentGatewayService.cs

Acción: Agregar método agnóstico y Value Objects

namespace ElectroLink.Subscriptions.Domain.Services;
public interface IPaymentGatewayService
{
    // ... métodos existentes ...
    
    /// <summary>
    /// Crea una sesión de checkout para procesar un pago de forma externa.
    /// La sesión genera una URL donde el usuario puede completar el pago.
    /// </summary>
    Task<CheckoutSession> CreateCheckoutSessionAsync(
        CheckoutSessionRequest request,
        CancellationToken cancellationToken = default);
}
// Value Objects para Checkout (agnósticos al proveedor)
public record CheckoutSessionRequest(
    UserId UserId,
    EmailAddress UserEmail,
    PlanId PlanId,
    Money Amount,
    Currency Currency,
    string SuccessUrl,
    string CancelUrl,
    CheckoutSessionMetadata? Metadata = null
);
public record CheckoutSession(
    CheckoutSessionId SessionId,        // ID generado por el gateway
    Uri CheckoutUrl,                     // URL donde el usuario completa el pago
    DateTime ExpiresAt,                  // Cuándo expira la sesión
    CheckoutSessionStatus Status,        // Estado de la sesión
    Money Amount,                        // Monto a cobrar
    Currency Currency                    // Moneda
);
public record CheckoutSessionId(string Value);
public record CheckoutSessionMetadata(
    string? CustomerId,
    string? SubscriptionId,
    Dictionary<string, string>? AdditionalData = null
);
public enum CheckoutSessionStatus
{
    Open,        // Sesión activa, esperando pago
    Complete,    // Pago completado
    Expired      // Sesión expirada
}
Justificación de los Value Objects:

CheckoutSessionRequest: Encapsula los datos necesarios para crear una sesión (dominio)
CheckoutSession: Representa el resultado (URL, expiración, etc.) - agnóstico
No menciona "Stripe" en ningún lado
Puede ser implementado por cualquier gateway (Stripe, PayPal, etc.)
Paso 2: Implementar en Infrastructure con Stripe
Archivo: Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs

Acción: Implementar el método usando Stripe.Checkout

namespace ElectroLink.Subscriptions.Infrastructure.PaymentGateway.Stripe;
using Stripe.Checkout;
using ElectroLink.Subscriptions.Domain.Services;
public partial class StripePaymentGatewayService : IPaymentGatewayService
{
    private readonly SessionService _checkoutSessionService;
    
    public async Task<CheckoutSession> CreateCheckoutSessionAsync(
        CheckoutSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Creating checkout session for user {UserId} and plan {PlanId}",
                request.UserId, request.PlanId);
            // Mapear de Domain → Stripe
            var options = new SessionCreateOptions
            {
                Mode = "subscription",  // Puede parametrizarse
                CustomerEmail = request.UserEmail.Value,
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        Price = request.PlanId.Value,  // Stripe Price ID
                        Quantity = 1
                    }
                },
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", request.UserId.Value.ToString() },
                    { "PlanId", request.PlanId.Value },
                    { "Source", "ElectroLink" }
                }
            };
            // Agregar metadata adicional si existe
            if (request.Metadata?.AdditionalData != null)
            {
                foreach (var (key, value) in request.Metadata.AdditionalData)
                {
                    options.Metadata[key] = value;
                }
            }
            // Llamar a Stripe
            var stripeSession = await _checkoutSessionService.CreateAsync(
                options,
                cancellationToken: cancellationToken);
            _logger.LogInformation(
                "Stripe checkout session created: {SessionId}",
                stripeSession.Id);
            // Mapear de Stripe → Domain
            return new CheckoutSession(
                SessionId: new CheckoutSessionId(stripeSession.Id),
                CheckoutUrl: new Uri(stripeSession.Url),
                ExpiresAt: stripeSession.ExpiresAt,
                Status: MapStripeSessionStatus(stripeSession.Status),
                Amount: request.Amount,
                Currency: request.Currency
            );
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, 
                "Stripe error creating checkout session for user {UserId}", 
                request.UserId);
            throw new PaymentGatewayException(
                "Failed to create checkout session in payment gateway", ex);
        }
    }
    private CheckoutSessionStatus MapStripeSessionStatus(string stripeStatus)
    {
        return stripeStatus switch
        {
            "open" => CheckoutSessionStatus.Open,
            "complete" => CheckoutSessionStatus.Complete,
            "expired" => CheckoutSessionStatus.Expired,
            _ => CheckoutSessionStatus.Open
        };
    }
}
Puntos Clave:

✅ Traduce de dominio (CheckoutSessionRequest) a Stripe (SessionCreateOptions)
✅ Traduce de Stripe (Session) a dominio (CheckoutSession)
✅ Maneja errores específicos de Stripe
✅ La capa de Application NO sabe nada de Stripe
Paso 3: Actualizar Application/CommandServices
Archivo: Application/Internal/CommandServices/StripeCheckoutCommandService.cs

ANTES (Incorrecto):

using ElectroLink.Subscriptions.Application.Internal.OutboundServices; // ← Mal
public class StripeCheckoutCommandService
{
    private readonly IStripeService _stripeService; // ← Acoplado a Stripe
    
    public async Task<CheckoutSessionResource> HandleAsync(
        CreateCheckoutSessionCommand command)
    {
        // Llama a IStripeService (Application)
        var session = await _stripeService.CreateCheckoutSession(...);
        return ...;
    }
}
DESPUÉS (Correcto):

using ElectroLink.Subscriptions.Domain.Services; // ← Correcto
public class CheckoutCommandService  // ← Renombrar (no mencionar Stripe)
{
    private readonly IPaymentGatewayService _paymentGateway; // ← Domain
    private readonly IUserRepository _userRepository;
    private readonly IPlanRepository _planRepository;
    private readonly ILogger<CheckoutCommandService> _logger;
    
    public CheckoutCommandService(
        IPaymentGatewayService paymentGateway,
        IUserRepository userRepository,
        IPlanRepository planRepository,
        ILogger<CheckoutCommandService> logger)
    {
        _paymentGateway = paymentGateway;
        _userRepository = userRepository;
        _planRepository = planRepository;
        _logger = logger;
    }
    
    public async Task<CheckoutSessionResource> HandleAsync(
        CreateCheckoutSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Creating checkout session for user {UserId} and plan {PlanId}",
            command.UserId, command.PlanId);
        // 1. Validar que el usuario existe
        var user = await _userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {command.UserId} not found");
        }
        // 2. Validar que el plan existe y está activo
        var plan = await _planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan == null || !plan.IsActive)
        {
            throw new BusinessException($"Plan {command.PlanId} is not available");
        }
        // 3. Crear request de dominio
        var checkoutRequest = new CheckoutSessionRequest(
            UserId: command.UserId,
            UserEmail: user.Email,
            PlanId: command.PlanId,
            Amount: plan.Price,
            Currency: plan.Currency,
            SuccessUrl: command.SuccessUrl,
            CancelUrl: command.CancelUrl,
            Metadata: new CheckoutSessionMetadata(
                CustomerId: null,  // Se creará después
                SubscriptionId: null,
                AdditionalData: new Dictionary<string, string>
                {
                    { "PlanName", plan.Name },
                    { "CreatedAt", DateTime.UtcNow.ToString("O") }
                }
            )
        );
        // 4. Llamar al gateway (abstracción)
        var checkoutSession = await _paymentGateway.CreateCheckoutSessionAsync(
            checkoutRequest,
            cancellationToken);
        _logger.LogInformation(
            "Checkout session {SessionId} created for user {UserId}",
            checkoutSession.SessionId.Value, command.UserId);
        // 5. Mapear a Resource para la respuesta
        return new CheckoutSessionResource(
            SessionId: checkoutSession.SessionId.Value,
            CheckoutUrl: checkoutSession.CheckoutUrl.ToString(),
            ExpiresAt: checkoutSession.ExpiresAt
        );
    }
}
Cambios Clave:

❌ NO usa IStripeService (Application)
✅ USA IPaymentGatewayService (Domain)
✅ Construye CheckoutSessionRequest con datos de dominio
✅ No sabe nada de Stripe
Paso 4: Refactor del Controller
Archivo: Interfaces/REST/CheckoutController.cs (NO StripeController)

ANTES (si existe en StripeController):

[ApiController]
[Route("api/stripe")]  // ← MAL: expone detalles de Stripe
public class StripeController : ControllerBase
{
    [HttpPost("checkout-session")]  // ← MAL: /api/stripe/checkout-session
    public async Task<IActionResult> CreateCheckoutSession(...)
    {
        // ...
    }
}
DESPUÉS (correcto):

[ApiController]
[Route("api/checkout")]  // ← Agnóstico, no menciona Stripe
public class CheckoutController : ControllerBase
{
    private readonly CheckoutCommandService _commandService;
    
    public CheckoutController(CheckoutCommandService commandService)
    {
        _commandService = commandService;
    }
    
    /// <summary>
    /// Crea una sesión de checkout para procesar un pago
    /// </summary>
    [HttpPost("session")]
    [ProducesResponseType(typeof(CheckoutSessionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCheckoutSession(
        [FromBody] CreateCheckoutSessionRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Transformar Request → Command (Assembler pattern)
        var command = CreateCheckoutSessionCommandFromResourceAssembler
            .ToCommand(request);
        
        // 2. Ejecutar comando
        var result = await _commandService.HandleAsync(command, cancellationToken);
        
        // 3. Retornar respuesta
        return CreatedAtAction(
            nameof(CreateCheckoutSession),
            new { sessionId = result.SessionId },
            result);
    }
}
Endpoint Final:

POST /api/checkout/session
NO:

POST /api/stripe/checkout-session  ← Expone detalle de implementación
Paso 5: Eliminar IStripeService
Acción: Una vez que todos los usages de IStripeService estén migrados:

Verificar que NO hay referencias:

# Buscar referencias a IStripeService
Get-ChildItem -Recurse -Filter *.cs | Select-String "IStripeService"
Si solo está en:

IStripeService.cs
 (definición)
StripePaymentGatewayService.cs (implementación que se eliminará)
Program.cs o DI registration
Eliminar archivos:

❌ Application/Internal/OutboundServices/IStripeService.cs
❌ Application/Internal/CommandServices/StripeCheckoutCommandService.cs (si existe)
Actualizar DI en Program.cs:

ANTES:

// Registrar IStripeService (ELIMINAR)
builder.Services.AddScoped<IStripeService, StripePaymentGatewayService>();
// Registrar IPaymentGatewayService
builder.Services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
DESPUÉS:

// Solo IPaymentGatewayService (Domain)
builder.Services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
// Renombrar CommandService si es necesario
builder.Services.AddScoped<CheckoutCommandService>();  // No "Stripe"CheckoutCommandService
Resumen de Cambios por Archivo
Archivo	Acción	Prioridad
Domain/Services/IPaymentGatewayService.cs	Agregar método CreateCheckoutSessionAsync + Value Objects	🔴 CRÍTICO
Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs	Implementar método usando Stripe.Checkout	🔴 CRÍTICO
Application/Internal/CommandServices/*CheckoutCommandService.cs	Cambiar de IStripeService a IPaymentGatewayService	🔴 CRÍTICO
Interfaces/REST/CheckoutController.cs	Asegurar que NO expone "Stripe" en la URL	🟡 MEDIO
Interfaces/REST/StripeController.cs	Mover endpoint a CheckoutController o eliminar	🟡 MEDIO
Application/Internal/OutboundServices/IStripeService.cs	ELIMINAR completamente	🔴 CRÍTICO
Program.cs	Actualizar DI registration	🔴 CRÍTICO
Orden de Implementación
Fase 1: Agregar Abstracción (30 min)
Agregar método y Value Objects en Domain/Services/IPaymentGatewayService.cs
Implementar en Infrastructure/PaymentGateway/Stripe/StripePaymentGatewayService.cs
Compilar y verificar que no hay errores
Fase 2: Migrar Application (1 hora)
Actualizar CommandService para usar IPaymentGatewayService
Actualizar DI en Program.cs
Compilar y verificar
Fase 3: Refactor Controllers (30 min)
Mover/Renombrar endpoint a CheckoutController
Verificar que URLs NO mencionan "Stripe"
Probar endpoints manualmente
Fase 4: Limpieza (30 min)
Buscar referencias a IStripeService
Eliminar 
IStripeService.cs
Eliminar registration de DI
Compilación final
Fase 5: Testing (30 min)
Unit tests de CheckoutCommandService (mockear IPaymentGatewayService)
Integration tests con Stripe Test Mode
Validar flujo completo
Tiempo Total Estimado: 3 horas

Validación Final
✅ Checklist de Migración Exitosa
 IPaymentGatewayService (Domain) tiene método CreateCheckoutSessionAsync
 StripePaymentGatewayService (Infrastructure) implementa el método usando Stripe
 CheckoutCommandService (Application) usa IPaymentGatewayService, NO IStripeService
 Controllers NO exponen "Stripe" en URLs públicas
 
IStripeService.cs
 ha sido completamente eliminado
 DI solo registra IPaymentGatewayService
 Compilación exitosa sin errores
 Tests pasan correctamente
✅ Pruebas de Regresión
Crear Checkout Session:

POST /api/checkout/session
{
  "userId": "guid",
  "planId": "plan_id",
  "successUrl": "https://app.com/success",
  "cancelUrl": "https://app.com/cancel"
}
Esperar: 201 Created con CheckoutSessionResource
Verificar: URL de Stripe funcional
Cambiar de Proveedor (futuro):

Debería poder cambiar a PayPal implementando IPaymentGatewayService
SIN modificar Application o Controllers
Preguntas Frecuentes
Q1: ¿Por qué CreateCheckoutSession es un concepto de dominio?
R: Porque representa la intención de negocio "iniciar un flujo de pago externo", independiente de la tecnología. PayPal, MercadoPago, etc. tienen conceptos equivalentes.

Q2: ¿Qué pasa con los webhooks de Stripe?
R: Los webhooks SON específicos de Stripe y deben quedarse en:

StripeWebhooksController (OK - es para webhooks)
Infrastructure/PaymentGateway/Stripe/Webhooks/* (OK - detalles de Stripe)
Q3: ¿Debo eliminar StripeController completamente?
R: Depende:

Si SOLO tiene CreateCheckoutSession → Mover a CheckoutController y eliminar
Si tiene otros endpoints administrativos de Stripe → Mantener pero NO exponerlo públicamente (quizás solo para admin/internal)
Q4: ¿Qué más tiene IStripeService que deba migrar?
R: Revisa TODOS los métodos de IStripeService:

Si son conceptos de dominio (customers, subscriptions, checkout) → Migrar a IPaymentGatewayService
Si son detalles administrativos de Stripe → Evaluar si son necesarios
Conclusión
El método CreateCheckoutSession DEBE estar en Domain porque:

✅ Es un concepto de negocio ("iniciar pago externo")
✅ No es específico de Stripe (otros gateways lo tienen)
✅ Application necesita esta abstracción para ser testeable
✅ Permite cambiar de proveedor sin modificar capas superiores
Siguiente Paso:
Comenzar con Fase 1: Agregar la abstracción en IPaymentGatewayService y su implementación en StripePaymentGatewayService.

