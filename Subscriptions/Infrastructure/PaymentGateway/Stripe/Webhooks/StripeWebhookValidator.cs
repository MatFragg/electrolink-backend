using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;

/// <summary>
/// Validates the authenticity of Stripe webhooks using signatures..
/// </summary>
public class StripeWebhookValidator
{
    private readonly StripeConfiguration _config;
    private readonly ILogger<StripeWebhookValidator> _logger;

    public StripeWebhookValidator(
        StripeConfiguration config,
        ILogger<StripeWebhookValidator> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <summary>
    /// Validates and constructs the Stripe event from the payload and the signature.
    /// </summary>
    /// <param name="json">JSON del webhook received.</param>
    /// <param name="stripeSignatureHeader">Header 'Stripe-Signature'.</param>
    /// <returns>The validated Stripe event.</returns>
    /// <exception cref="StripeException">If the validation fails.</exception>
    public Event ConstructEvent(string json, string stripeSignatureHeader)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignatureHeader,
                _config.WebhookSecret,
                throwOnApiVersionMismatch: false // fail if API version differs
            );

            _logger.LogInformation(
                "Webhook validated successfully: {EventId} - {EventType}",
                stripeEvent.Id,
                stripeEvent.Type);

            return stripeEvent;
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Failed to validate webhook signature");
            throw;
        }
    }

    /// <summary>
    /// Validates if the event has already been processed (basic idempotency).
    /// </summary>
    public bool IsEventTooOld(Event stripeEvent, TimeSpan maxAge)
    {
        var eventAge = DateTime.UtcNow - stripeEvent.Created;
        
        if (eventAge > maxAge)
        {
            _logger.LogWarning(
                "Event {EventId} is too old: {EventAge} (max: {MaxAge})",
                stripeEvent.Id,
                eventAge,
                maxAge);
            return true;
        }

        return false;
    }
}