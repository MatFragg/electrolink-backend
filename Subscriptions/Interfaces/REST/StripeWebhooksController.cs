using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Shared.Infrastructure;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/webhooks/stripe")]
public class StripeWebhookController(
    IConfiguration configuration,
    ILogger<StripeWebhookController> logger,
    ISubscriptionCommandService commandService,
    IPaymentProvider paymentProvider) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();
        var webhookSecret = configuration["Stripe:WebhookSecret"]!;

        WebhookEvent webhookEvent;
        try
        {
            webhookEvent = await paymentProvider.ValidateWebhookSignatureAsync(json, signature, webhookSecret);
        }
        catch (Exception ex)
        {
            logger.LogWarning("Invalid Stripe webhook signature: {Message}", ex.Message);
            return BadRequest(new { error = "Invalid signature" });
        }

        try
        {
            Func<WebhookEvent, Task> handler = webhookEvent.EventType switch
            {
                "checkout.session.completed" => HandleCheckoutSessionCompletedAsync,
                "invoice.payment_succeeded" => HandleInvoicePaymentSucceededAsync,
                "invoice.payment_failed" => HandleInvoicePaymentFailedAsync,
                "customer.subscription.deleted" => HandleCustomerSubscriptionDeletedAsync,
                _ => null!
            };

            if (handler is null)
            {
                logger.LogInformation("Unhandled Stripe event type: {EventType}", webhookEvent.EventType);
                return Ok();
            }

            await handler(webhookEvent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Stripe webhook {EventType}", webhookEvent.EventType);
            return StatusCode(500);
        }

        return Ok();
    }

    private async Task HandleCheckoutSessionCompletedAsync(WebhookEvent webhookEvent)
    {
        var doc = JsonDocument.Parse(webhookEvent.RawJson);
        var root = doc.RootElement;
        var dataObj = root.GetProperty("data").GetProperty("object");

        var metadata = dataObj.TryGetProperty("metadata", out var meta) ? meta : default;
        var isEnterprise = metadata.ValueKind != JsonValueKind.Undefined
                           && metadata.TryGetProperty("isEnterprise", out var ie)
                           && ie.GetString() == "true";

        var customerId = dataObj.TryGetProperty("customer", out var cust) ? cust.GetString()! : null;
        var subscriptionId = dataObj.TryGetProperty("subscription", out var sub) ? sub.GetString()! : null;
        var amountTotal = dataObj.TryGetProperty("amount_total", out var amt) ? (int)(amt.GetInt64() / 100) : 0;
        var currency = dataObj.TryGetProperty("currency", out var cur) ? cur.GetString()! : "usd";

        if (isEnterprise)
        {
            await commandService.Handle(new ActivateEnterpriseSubscriptionPendingInstallationCommand(
                StripeCustomerId: customerId!,
                StripeSubscriptionId: subscriptionId!,
                StripeInvoiceId: $"cs_{Guid.NewGuid():N}",
                AmountPaid: amountTotal,
                Currency: currency,
                PeriodStart: DateTime.UtcNow,
                PeriodEnd: DateTime.UtcNow.AddYears(1)));
        }
        else
        {
            var billingCycle = metadata.ValueKind != JsonValueKind.Undefined
                               && metadata.TryGetProperty("billingCycle", out var bc)
                               ? bc.GetString()!
                               : "MONTHLY";

            await commandService.Handle(new ActivateSubscriptionCommand(
                StripeCustomerId: customerId!,
                StripeSubscriptionId: subscriptionId!,
                StripeInvoiceId: $"cs_{Guid.NewGuid():N}",
                BillingCycle: billingCycle,
                AmountPaid: amountTotal,
                Currency: currency,
                PeriodStart: DateTime.UtcNow,
                PeriodEnd: DateTime.UtcNow.AddMonths(billingCycle == "MONTHLY" ? 1 : 12)));
        }
    }

    private async Task HandleInvoicePaymentSucceededAsync(WebhookEvent webhookEvent)
    {
        var doc = JsonDocument.Parse(webhookEvent.RawJson);
        var root = doc.RootElement;
        var dataObj = root.GetProperty("data").GetProperty("object");

        var billingReason = dataObj.TryGetProperty("billing_reason", out var br) ? br.GetString()! : string.Empty;
        var invoiceId = dataObj.TryGetProperty("id", out var id) ? id.GetString()! : string.Empty;
        var customerId = dataObj.TryGetProperty("customer", out var cust) ? cust.GetString()! : null;
        var subscriptionId = dataObj.TryGetProperty("subscription", out var sub) ? sub.GetString()! : null;
        var amountPaid = dataObj.TryGetProperty("amount_paid", out var amt) ? (int)(amt.GetInt64() / 100) : 0;
        var currency = dataObj.TryGetProperty("currency", out var cur) ? cur.GetString()! : "usd";
        var periodStart = DateTime.UtcNow;
        var periodEnd = DateTime.UtcNow.AddMonths(1);

        if (dataObj.TryGetProperty("lines", out var lines) && lines.TryGetProperty("data", out var dataArray) && dataArray.GetArrayLength() > 0)
        {
            var line = dataArray[0];
            if (line.TryGetProperty("period", out var period))
            {
                if (period.TryGetProperty("start", out var ps))
                    periodStart = ps.TryGetDateTime(out var dt1) ? dt1 : DateTime.UtcNow;
                if (period.TryGetProperty("end", out var pe))
                    periodEnd = pe.TryGetDateTime(out var dt2) ? dt2 : DateTime.UtcNow.AddMonths(1);
            }
        }

        if (billingReason == "subscription_create")
        {
            var billingCycle = "MONTHLY";
            if (dataObj.TryGetProperty("lines", out var lines2) && lines2.TryGetProperty("data", out var dataArray2) && dataArray2.GetArrayLength() > 0)
            {
                var line = dataArray2[0];
                if (line.TryGetProperty("price", out var price) && price.TryGetProperty("recurring", out var recurring) && recurring.TryGetProperty("interval", out var interval))
                {
                    billingCycle = interval.GetString() == "month" ? "MONTHLY" : "ANNUAL";
                }
            }

            await commandService.Handle(new ActivateSubscriptionCommand(
                StripeCustomerId: customerId!,
                StripeSubscriptionId: subscriptionId!,
                StripeInvoiceId: invoiceId,
                BillingCycle: billingCycle,
                AmountPaid: amountPaid,
                Currency: currency,
                PeriodStart: periodStart,
                PeriodEnd: periodEnd));
        }
        else if (billingReason == "subscription_cycle")
        {
            await commandService.Handle(new RecordSuccessfulRenewalCommand(
                StripeSubscriptionId: subscriptionId!,
                StripeInvoiceId: invoiceId,
                AmountPaid: amountPaid,
                Currency: currency,
                NewPeriodStart: periodStart,
                NewPeriodEnd: periodEnd));
        }
    }

    private async Task HandleInvoicePaymentFailedAsync(WebhookEvent webhookEvent)
    {
        var doc = JsonDocument.Parse(webhookEvent.RawJson);
        var root = doc.RootElement;
        var dataObj = root.GetProperty("data").GetProperty("object");

        var subscriptionId = dataObj.TryGetProperty("subscription", out var sub) ? sub.GetString()! : null;
        var invoiceId = dataObj.TryGetProperty("id", out var id) ? id.GetString()! : string.Empty;
        var amountDue = dataObj.TryGetProperty("amount_due", out var amt) ? (int)(amt.GetInt64() / 100) : 0;
        var currency = dataObj.TryGetProperty("currency", out var cur) ? cur.GetString()! : "usd";

        if (subscriptionId is null) return;

        await commandService.Handle(new StartGracePeriodCommand(
            StripeSubscriptionId: subscriptionId,
            StripeInvoiceId: invoiceId,
            AmountDue: amountDue,
            Currency: currency,
            FailedAt: DateTime.UtcNow));
    }

    private async Task HandleCustomerSubscriptionDeletedAsync(WebhookEvent webhookEvent)
    {
        var doc = JsonDocument.Parse(webhookEvent.RawJson);
        var root = doc.RootElement;
        var dataObj = root.GetProperty("data").GetProperty("object");

        var subscriptionId = dataObj.TryGetProperty("id", out var id) ? id.GetString()! : null;
        var cancellationDetails = dataObj.TryGetProperty("cancellation_details", out var cd) ? cd : default;
        var reason = cancellationDetails.ValueKind != JsonValueKind.Undefined
                     && cancellationDetails.TryGetProperty("reason", out var r)
                     ? r.GetString() == "payment_failed" ? "PAYMENT_FAILURE" : "VOLUNTARY_CANCELLATION"
                     : "VOLUNTARY_CANCELLATION";

        if (subscriptionId is null) return;

        await commandService.Handle(new DegradeSubscriptionCommand(
            StripeSubscriptionId: subscriptionId,
            Reason: reason,
            DegradedAt: DateTime.UtcNow));
    }
}
