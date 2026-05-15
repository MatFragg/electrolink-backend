using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/webhooks/stripe")]
public class StripeWebhookController(IConfiguration configuration, ILogger<StripeWebhookController> logger, ISubscriptionCommandService commandService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = configuration["Stripe:WebhookSecret"]!;

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                webhookSecret);
        }
        catch (StripeException ex)
        {
            logger.LogWarning("Invalid Stripe webhook signature: {Message}", ex.Message);
            return BadRequest();
        }

        try
        {
            await DispatchWebhookEvent(stripeEvent);
        }
        catch (Exception ex)
        {
            // Retornar 500 para que Stripe reintente el webhook
            logger.LogError(ex, "Error processing Stripe webhook {EventType}", stripeEvent.Type);
            return StatusCode(500);
        }

        return Ok();
    }

    private async Task DispatchWebhookEvent(Event stripeEvent)
    {
        switch (stripeEvent.Type)
        {
            case "invoice.payment_succeeded":
            {
                var invoice = stripeEvent.Data.Object as Invoice;
                var line = invoice?.Lines?.Data?.FirstOrDefault();
                if (line == null) return;
                
                if (invoice!.BillingReason == "subscription_create")
                {
                    await commandService.Handle(new ActivateSubscriptionCommand(
                        StripeCustomerId: invoice.CustomerId,
                        StripeSubscriptionId: line.SubscriptionId!,
                        StripeInvoiceId: invoice.Id,
                        BillingCycle: await ResolveBillingCycleAsync(invoice),
                        AmountPaid: (int)invoice.AmountPaid,
                        Currency: invoice.Currency,
                        PeriodStart: line.Period.Start,
                        PeriodEnd: line.Period.End));
                }
                else if (invoice.BillingReason == "subscription_cycle")
                {
                    await commandService.Handle(new RecordSuccessfulRenewalCommand(
                        StripeSubscriptionId: line.SubscriptionId!,
                        StripeInvoiceId: invoice.Id,
                        AmountPaid: (int)invoice.AmountPaid,
                        Currency: invoice.Currency,
                        NewPeriodStart: line.Period.Start,
                        NewPeriodEnd: line.Period.End));
                }
                break;
            }
            case "invoice.payment_failed":
            {
                var invoice = stripeEvent.Data.Object as Invoice;
                await commandService.Handle(new StartGracePeriodCommand(
                    StripeSubscriptionId: invoice!.Lines!.Data[0].SubscriptionId!,
                    StripeInvoiceId:      invoice.Id,
                    AmountDue:            (int)invoice.AmountDue,
                    Currency:             invoice.Currency,
                    FailedAt:             DateTime.UtcNow));
                break;
            }
            case "customer.subscription.deleted":
            {
                var subscription = stripeEvent.Data.Object as Subscription;
                var reason = subscription!.CancellationDetails?.Reason == "payment_failed"
                    ? "PAYMENT_FAILURE"
                    : "VOLUNTARY_CANCELLATION";

                await commandService.Handle(new DegradeSubscriptionCommand(
                    StripeSubscriptionId: subscription.Id,
                    Reason: reason,
                    DegradedAt: DateTime.UtcNow));
                break;
            }
            case "customer.subscription.updated":
            {
                var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                var item = subscription?.Items?.Data?.FirstOrDefault();
                if (item?.Price?.Recurring == null) return;
                
                var newCycle = item.Price.Recurring.Interval == "month" ? "MONTHLY" : "ANNUAL";

                await commandService.Handle(new UpdateBillingCycleCommand(
                    StripeSubscriptionId: subscription!.Id,
                    NewBillingCycle: newCycle,
                    NewPeriodStart: item.CurrentPeriodStart,
                    NewPeriodEnd: item.CurrentPeriodEnd));
                break;
            }
            default:
                logger.LogInformation("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                break;
        }
    }

    private async Task<string> ResolveBillingCycleAsync(Invoice invoice)
    {
        var subscriptionService = new SubscriptionService();
        var subscription = await subscriptionService.GetAsync(invoice!.Lines!.Data[0].SubscriptionId!);

        var item = subscription.Items.Data.FirstOrDefault();
        var interval = item?.Price?.Recurring?.Interval;

        return interval == "month" ? "MONTHLY" : "ANNUAL";
    }
}