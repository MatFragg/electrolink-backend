using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;

/// <summary>
/// Procesa eventos de webhook de Stripe y delega acciones al domain.
/// </summary>
public class StripeWebhookEventProcessor
{
    private readonly IWebhookEventRepository _webhookEventRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<StripeWebhookEventProcessor> _logger;

    public StripeWebhookEventProcessor(
        IWebhookEventRepository webhookEventRepository,
        IMediator mediator,
        ILogger<StripeWebhookEventProcessor> logger)
    {
        _webhookEventRepository = webhookEventRepository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Procesa un evento de Stripe de forma idempotente.
    /// </summary>
    public async Task<bool> ProcessEventAsync(Event stripeEvent, string rawJson)
    {
        var webhookEventId = new WebhookEventId(stripeEvent.Id);

        // 1. Verificar idempotencia: ¿Ya procesamos este evento?
        var existingEvent = await _webhookEventRepository.FindByIdAsync(webhookEventId);
        
        if (existingEvent != null)
        {
            if (existingEvent.ShouldSkipProcessing())
            {
                _logger.LogInformation(
                    "Evento {EventId} ya fue procesado o excedió reintentos. Saltando.",
                    webhookEventId);
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
            // 5. Registrar fallo y incrementar reintentos
            _logger.LogError(ex, "Error procesando evento {EventId}: {EventType}", 
                webhookEventId, stripeEvent.Type);

            existingEvent.RecordFailedAttempt(ex.Message);
            _webhookEventRepository.Update(existingEvent);
            await _webhookEventRepository.SaveChangesAsync();

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
            case Events.CustomerSubscriptionCreated:
                await HandleSubscriptionCreatedAsync(stripeEvent);
                break;

            case Events.CustomerSubscriptionUpdated:
                await HandleSubscriptionUpdatedAsync(stripeEvent);
                break;

            case Events.CustomerSubscriptionDeleted:
                await HandleSubscriptionDeletedAsync(stripeEvent);
                break;

            case Events.InvoicePaymentSucceeded:
                await HandleInvoicePaymentSucceededAsync(stripeEvent);
                break;

            case Events.InvoicePaymentFailed:
                await HandleInvoicePaymentFailedAsync(stripeEvent);
                break;

            case Events.CustomerSubscriptionTrialWillEnd:
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

    #region Event Handlers

    private async Task HandleSubscriptionCreatedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null) return;

        _logger.LogInformation(
            "Procesando customer.subscription.created: {SubscriptionId}",
            subscription.Id);

        // Crear comando para sincronizar la suscripción
        var command = new SyncSubscriptionFromStripeCommand(
            subscription.Id,
            subscription.CustomerId,
            subscription.Items.Data[0].Price.Id,
            subscription.Status,
            subscription.CurrentPeriodStart,
            subscription.CurrentPeriodEnd,
            subscription.TrialEnd,
            subscription.CanceledAt,
            subscription.CancelAt,
            subscription.CancelAtPeriodEnd
        );

        await _mediator.Send(command);
    }

    private async Task HandleSubscriptionUpdatedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null) return;

        _logger.LogInformation(
            "Procesando customer.subscription.updated: {SubscriptionId}",
            subscription.Id);

        var command = new SyncSubscriptionFromStripeCommand(
            subscription.Id,
            subscription.CustomerId,
            subscription.Items.Data[0].Price.Id,
            subscription.Status,
            subscription.CurrentPeriodStart,
            subscription.CurrentPeriodEnd,
            subscription.TrialEnd,
            subscription.CanceledAt,
            subscription.CancelAt,
            subscription.CancelAtPeriodEnd
        );

        await _mediator.Send(command);
    }

    private async Task HandleSubscriptionDeletedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null) return;

        _logger.LogInformation(
            "Procesando customer.subscription.deleted: {SubscriptionId}",
            subscription.Id);

        // La suscripción fue cancelada o expiró en Stripe
        var command = new SyncSubscriptionFromStripeCommand(
            subscription.Id,
            subscription.CustomerId,
            subscription.Items.Data[0].Price.Id,
            "canceled", // Forzar estado cancelado
            subscription.CurrentPeriodStart,
            subscription.CurrentPeriodEnd,
            null,
            DateTime.UtcNow,
            null,
            false
        );

        await _mediator.Send(command);
    }

    private async Task HandleInvoicePaymentSucceededAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null || invoice.SubscriptionId == null) return;

        _logger.LogInformation(
            "Procesando invoice.payment_succeeded: Invoice {InvoiceId} - Subscription {SubscriptionId}",
            invoice.Id,
            invoice.SubscriptionId);

        // Buscar la suscripción en nuestra DB por StripeSubscriptionId
        var command = new ProcessPaymentCommand(
            Guid.Empty, // Se buscará por StripeSubscriptionId
            invoice.AmountPaid / 100m, // Stripe usa centavos
            invoice.Currency,
            DateTime.UtcNow,
            EPaymentStatus.Success,
            invoice.PaymentIntentId ?? invoice.Id,
            $"Payment succeeded for invoice {invoice.Id}"
        );

        await _mediator.Send(command);
    }

    private async Task HandleInvoicePaymentFailedAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null || invoice.SubscriptionId == null) return;

        _logger.LogWarning(
            "Procesando invoice.payment_failed: Invoice {InvoiceId} - Subscription {SubscriptionId}",
            invoice.Id,
            invoice.SubscriptionId);

        var command = new ProcessPaymentCommand(
            Guid.Empty,
            invoice.AmountDue / 100m,
            invoice.Currency,
            DateTime.UtcNow,
            EPaymentStatus.Failed,
            invoice.PaymentIntentId ?? invoice.Id,
            $"Payment failed: {invoice.LastFinalizationError?.Message ?? "Unknown error"}"
        );

        await _mediator.Send(command);
    }

    private async Task HandleTrialWillEndAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null || subscription.TrialEnd == null) return;

        _logger.LogInformation(
            "Procesando customer.subscription.trial_will_end: {SubscriptionId} - Trial ends: {TrialEnd}",
            subscription.Id,
            subscription.TrialEnd);

        // TODO: Crear Integration Event para notificar al usuario
        // TrialEndingSoonIntegrationEvent
    }

    #endregion
}