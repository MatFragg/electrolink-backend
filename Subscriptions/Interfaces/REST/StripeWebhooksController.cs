using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

/// <summary>
/// Controller that handles Stripe Webhooks.
/// Public endpoint (no authentication) but validated with signature.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/v1/stripe/webhooks")]
public class StripeWebhooksController(StripeWebhookValidator webhookValidator,
    StripeWebhookEventProcessor eventProcessor, 
    ILogger<StripeWebhooksController> logger) : ControllerBase
{

    /// <summary>
    /// Endpoint to handle Stripe Webhooks.
    /// URL to configure in Stripe Dashboard: https://tudominio.com/api/v1/stripe/webhooks
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        try
        {
            // 1. Obtain the signature header
            var stripeSignature = Request.Headers["Stripe-Signature"].ToString();
            
            if (string.IsNullOrEmpty(stripeSignature))
            {
                logger.LogWarning("Webhook received without Stripe-Signature header");
                return BadRequest(new { error = "Missing Stripe-Signature header" });
            }

            // 2. Validate the webhook using the signature
            var stripeEvent = webhookValidator.ConstructEvent(json, stripeSignature);

            // 3. Check if the event is too old (protection against replay attacks)
            if (webhookValidator.IsEventTooOld(stripeEvent, TimeSpan.FromHours(24)))
            {
                logger.LogWarning("Webhook event {EventId} is too old", stripeEvent.Id);
                return Ok(new { message = "Event too old, ignored" });
            }

            // 4. Process the event idempotently
            var processed = await eventProcessor.ProcessEventAsync(stripeEvent, json);

            if (processed)
            {
                logger.LogInformation("Webhook {EventId} processed successfully", stripeEvent.Id);
                return Ok(new { received = true, eventId = stripeEvent.Id });
            }
            else
            {
                logger.LogInformation("Webhook {EventId} was already processed", stripeEvent.Id);
                return Ok(new { received = true, eventId = stripeEvent.Id, note = "Already processed" });
            }
        }
        catch (Stripe.StripeException ex)
        {
            logger.LogError(ex, "Error validating Stripe webhook");
            return BadRequest(new { error = "Invalid webhook signature" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Stripe webhook");

            // IMPORTANT: Return 200 even if internal processing fails
            // to prevent Stripe from retrying indefinitely
            // The log in WebhookEvent will save the error for manual review
            return Ok(new {
                received = true, 
                error = "Processing error logged, will retry internally" 
            });
        }
    }

    /// <summary>
    /// Health check endpoint to verify that the webhook is active.
    /// Should not be configured in Stripe - for debugging only.
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new { status = "Webhook endpoint is active" });
    }
}