using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;

/// <summary>
/// Procesa eventos de webhook de Stripe y delega acciones al domain.
/// </summary>
public class StripeWebhookEventProcessor
{
    private readonly IWebhookEventRepository _webhookEventRepository;
    private readonly IPlanRepository _planRepository; // <- Declarado
    private readonly IMediator _mediator;
    private readonly ILogger<StripeWebhookEventProcessor> _logger;
    private readonly string _stripeSecretKey;
    private ISubscriptionCommandService _subscriptionCommandService;
    
    // 💡 CONSTRUCTOR CORREGIDO
    public StripeWebhookEventProcessor(
        IWebhookEventRepository webhookEventRepository, 
        IPlanRepository planRepository, // <- ¡Nuevo parámetro necesario!
        IMediator mediator,
        ILogger<StripeWebhookEventProcessor> logger,
        IConfiguration configuration,
        ISubscriptionCommandService subscriptionCommandService
    )
    {
        _webhookEventRepository = webhookEventRepository;
        _planRepository = planRepository; // <- Asignado
        _mediator = mediator;
        _logger = logger;
        _stripeSecretKey = configuration["Stripe:SecretKey"] 
                           ?? throw new InvalidOperationException("Stripe SecretKey missing in configuration for Webhook Processor.");
        _subscriptionCommandService = subscriptionCommandService;
        // 💡 Inicializa servicios de Stripe aquí. Esto es seguro y permite la inyección de dependencias si lo necesitas en el futuro.
        // Si usas la versión 45.3.0 de Stripe.net, la inicialización es simple:
    }
    
    /// <summary>
    /// Procesa un evento de Stripe de forma idempotente.
    /// </summary>
    public async Task<bool> ProcessEventAsync(Event stripeEvent, string rawJson)
    {
        var webhookEventId = new WebhookEventId(stripeEvent.Id);
        WebhookEvent existingEvent = null; // Inicializa fuera del if/else

        // 1. Verificar idempotencia (Código omitido por brevedad, pero asume que funciona)

        existingEvent = await _webhookEventRepository.FindByIdAsync(webhookEventId);
        
        if (existingEvent != null)
        {
            if (existingEvent.ShouldSkipProcessing())
            {
                _logger.LogInformation("Evento {EventId} ya fue procesado o excedió reintentos. Saltando.", webhookEventId);
                return false;
            }
        }
        else
        {
            // 2. Crear registro del evento en DB
            existingEvent = new WebhookEvent(
                webhookEventId,
                stripeEvent.Type,
                rawJson,
                stripeEvent.Created);

            await _webhookEventRepository.AddAsync(existingEvent);
            await _webhookEventRepository.SaveChangesAsync();
        }

        // 3. Procesar el evento según su tipo
        try
        {
            await RouteEventToHandlerAsync(stripeEvent);

            // 4. Marcar como procesado exitosamente
            existingEvent.MarkAsProcessed();
            _webhookEventRepository.Update(existingEvent);
            await _webhookEventRepository.SaveChangesAsync();

            _logger.LogInformation("Evento {EventId} procesado exitosamente", webhookEventId);
            return true;
        }
        catch (Exception ex)
        {
            // 🚨 MODIFICACIÓN CLAVE: Revelar la excepción oculta del 400
            
            // 5. Registrar fallo y incrementar reintentos
            _logger.LogError(ex, 
                "🛑 FALLO AL PROCESAR EVENTO DE NEGOCIO {EventId} ({EventType}). El error fue: {ErrorMessage}", 
                webhookEventId, stripeEvent.Type, ex.Message);

            // Registrar el intento fallido en la DB
            string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            existingEvent.RecordFailedAttempt(errorMessage);
            _webhookEventRepository.Update(existingEvent);
            await _webhookEventRepository.SaveChangesAsync();

            // Relanzar la excepción para que el Controller devuelva 400/500 a Stripe
            throw;
        }
    }

    /// <summary>
    /// Enruta el evento de Stripe al handler correspondiente.
    /// </summary>
    private async Task RouteEventToHandlerAsync(Event stripeEvent)
    {
        switch (stripeEvent.Type)
        {
            case EventTypes.CustomerSubscriptionCreated:
                await HandleSubscriptionCreatedAsync(stripeEvent);
                break;

            case EventTypes.CustomerSubscriptionUpdated:
                await HandleSubscriptionUpdatedAsync(stripeEvent);
                break;

            case EventTypes.CustomerSubscriptionDeleted:
                await HandleSubscriptionDeletedAsync(stripeEvent);
                break;

            case EventTypes.InvoicePaymentSucceeded:
                await HandleInvoicePaymentSucceededAsync(stripeEvent);
                break;

            case EventTypes.InvoicePaymentFailed:
                await HandleInvoicePaymentFailedAsync(stripeEvent);
                break;

            case EventTypes.CustomerSubscriptionTrialWillEnd:
                await HandleTrialWillEndAsync(stripeEvent);
                break;

            default:
                _logger.LogInformation(
                    "Evento no manejado: {EventType} - {EventId}",
                    stripeEvent.Type,
                    stripeEvent.Id);
                break;
        }
    }

    private async Task HandleSubscriptionCreatedAsync(Event stripeEvent)
    {
        var subscriptionStripe = stripeEvent.Data.Object as Subscription;
        if (subscriptionStripe == null) return;
        
        try
        {
            var (localUserId, planId, customerId, subId, initialStatus, trialEnd) = 
                await GetSubscriptionCreationDataAsync(subscriptionStripe);

            // 2. Crear el Comando de Creación de Suscripción
            var command = new CreateSubscriptionCommand(
                localUserId, 
                planId,
                (subscriptionStripe.Items.Data.FirstOrDefault()?.CurrentPeriodStart ?? DateTimeOffset.MinValue).UtcDateTime,
                (subscriptionStripe.Items.Data.FirstOrDefault()?.CurrentPeriodEnd ?? DateTimeOffset.MinValue).UtcDateTime,
                customerId,
                subId,
                initialStatus,
                trialEnd
            );
            
            await _subscriptionCommandService.Handle(command);
            
            _logger.LogInformation("✅ Suscripción local creada exitosamente para User {UserId} con Stripe Sub ID {StripeId}", 
                                  localUserId, subId);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Missing LocalUserId"))
        {
            _logger.LogError(ex, "🛑 ERROR DE DATOS: Metadata LocalUserId faltante para Customer {Id}", subscriptionStripe.CustomerId);
            throw; // Relanza 400
        }
        catch (Exception ex) when (ex is Npgsql.PostgresException || ex.InnerException is Npgsql.PostgresException || ex is Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            _logger.LogError(ex, "🛑 FALLO DE BASE DE DATOS (FK/NOT NULL) al crear Subscription para User {Id}. CAUSA: {Message}", 
                             subscriptionStripe.CustomerId, ex.InnerException?.Message ?? ex.Message);
            throw; // Relanza 400
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FALLO GENÉRICO al procesar SubscriptionCreated: {Id}.", subscriptionStripe.CustomerId);
            throw;
        }
    }
    private async Task HandleSubscriptionUpdatedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null)
        {
            _logger.LogWarning("No se pudo deserializar Subscription en evento {EventId}", stripeEvent.Id);
            return;
        }

        _logger.LogInformation(
            "Procesando customer.subscription.updated: {SubscriptionId}, Status: {Status}",
            subscription.Id,
            subscription.Status);

        // await mediator.Publish(new SubscriptionUpdatedDomainEvent(subscription.Id, subscription.Status));
    }

    private async Task HandleSubscriptionDeletedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null)
        {
            _logger.LogWarning("No se pudo deserializar Subscription en evento {EventId}", stripeEvent.Id);
            return;
        }

        _logger.LogInformation(
            "Procesando customer.subscription.deleted: {SubscriptionId}",
            subscription.Id);

        // await mediator.Publish(new SubscriptionDeletedDomainEvent(subscription.Id));
    }

    private async Task HandleInvoicePaymentSucceededAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null)
        {
            _logger.LogWarning("No se pudo deserializar Invoice en evento {EventId}", stripeEvent.Id);
            return;
        }

        // Obtener subscription ID desde las líneas del invoice
        var subscriptionId = invoice.Lines?.Data?.FirstOrDefault()?.SubscriptionId;
        
        _logger.LogInformation(
            "Procesando invoice.payment_succeeded: {InvoiceId} para Customer {CustomerId}, Subscription {SubscriptionId}",
            invoice.Id,
            invoice.Customer,
            subscriptionId ?? "N/A");

        // await mediator.Publish(new InvoicePaymentSucceededDomainEvent(invoice.Id, subscriptionId));
    }

    private async Task HandleInvoicePaymentFailedAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null)
        {
            _logger.LogWarning("No se pudo deserializar Invoice en evento {EventId}", stripeEvent.Id);
            return;
        }

        // Obtener subscription ID desde las líneas del invoice
        var subscriptionId = invoice.Lines?.Data?.FirstOrDefault()?.SubscriptionId;
        
        _logger.LogInformation(
            "Procesando invoice.payment_failed: {InvoiceId} para Customer {CustomerId}, Subscription {SubscriptionId}",
            invoice.Id,
            invoice.Customer,
            subscriptionId ?? "N/A");

        // await mediator.Publish(new InvoicePaymentFailedDomainEvent(invoice.Id, subscriptionId));
    }

    private async Task HandleTrialWillEndAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null)
        {
            _logger.LogWarning("No se pudo deserializar Subscription en evento {EventId}", stripeEvent.Id);
            return;
        }

        _logger.LogInformation(
            "Procesando customer.subscription.trial_will_end: {SubscriptionId}, Trial End: {TrialEnd}",
            subscription.Id,
            subscription.TrialEnd);

        // await mediator.Publish(new SubscriptionTrialWillEndDomainEvent(subscription.Id, subscription.TrialEnd));
    }
    
    private async Task<(UserId UserId, Guid PlanId, PaymentGatewayCustomerId CustomerId, PaymentGatewaySubscriptionId SubscriptionId, ESubscriptionStatus Status, DateTime? TrialEnd)> GetSubscriptionCreationDataAsync(Subscription subscriptionStripe) {
        // CUIDADO: Este método solo debe ejecutarse si la clave API (_stripeSecretKey) está disponible.

        // 1. Obtener Cliente de Stripe y Metadata
        var client = new StripeClient(_stripeSecretKey);
        var customer = await new CustomerService(client).GetAsync(subscriptionStripe.CustomerId);
        
        // Validar y obtener el LocalUserId
        if (!customer.Metadata.TryGetValue("LocalUserId", out var userIdString) || 
            !int.TryParse(userIdString, out var localUserId))
        {
            // Esto será capturado por el try/catch del método padre.
            throw new InvalidOperationException($"Missing LocalUserId metadata for Stripe Customer {customer.Id}"); 
        }
        
        // 2. Obtener el Plan Local
        var stripePriceId = subscriptionStripe.Items.Data.FirstOrDefault()?.Price?.Id;
        if (string.IsNullOrEmpty(stripePriceId)) 
        {
            // En un escenario real, esto podría ser un error 400.
            throw new InvalidOperationException("Price ID (Plan ID) not found in Stripe subscription."); 
        }

        var planLocal = await _planRepository.FindByPaymentGatewayPriceIdAsync(new PaymentGatewayPriceId(stripePriceId)); 
        if (planLocal == null) 
        {
            throw new InvalidOperationException($"Local Plan not found for Stripe Price ID: {stripePriceId}.");
        }

        // 3. Mapeo final
        ESubscriptionStatus initialStatus = subscriptionStripe.Status.ToLowerInvariant() == "trialing" 
            ? ESubscriptionStatus.Trial : ESubscriptionStatus.Active;

        // 4. Retornar todos los datos requeridos como una tupla
        return (
            UserId: new UserId(localUserId),
            PlanId: planLocal.Id.Value,
            CustomerId: new PaymentGatewayCustomerId(subscriptionStripe.CustomerId),
            SubscriptionId: new PaymentGatewaySubscriptionId(subscriptionStripe.Id),
            Status: initialStatus,
            TrialEnd: (DateTime?)subscriptionStripe.TrialEnd
        );
    }
}

// Clase de constantes para EventTypes (si no existe en tu versión)
public static class EventTypes
{
    public const string CustomerSubscriptionCreated = "customer.subscription.created";
    public const string CustomerSubscriptionUpdated = "customer.subscription.updated";
    public const string CustomerSubscriptionDeleted = "customer.subscription.deleted";
    public const string InvoicePaymentSucceeded = "invoice.payment_succeeded";
    public const string InvoicePaymentFailed = "invoice.payment_failed";
    public const string CustomerSubscriptionTrialWillEnd = "customer.subscription.trial_will_end";
}