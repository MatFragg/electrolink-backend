﻿using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;

public class StripeWebhookEventProcessor(
    IWebhookEventRepository webhookEventRepository,
    ISubscriptionCommandService subscriptionCommandService,
    ILogger<StripeWebhookEventProcessor> logger)
{
    public async Task<bool> ProcessEventAsync(Event stripeEvent, string rawJson)
    {
        var eventId = new WebhookEventId(stripeEvent.Id);
        var existing = await webhookEventRepository.FindByIdAsync(eventId);

        if (existing != null && existing.ShouldSkipProcessing())
        {
            logger.LogInformation("Webhook {EventId} already processed or max attempts reached.", eventId.Value);
            return false;
        }

        var webhookEvent = existing ?? new WebhookEvent(eventId, stripeEvent.Type, rawJson, stripeEvent.Created);

        if (existing == null)
        {
            await webhookEventRepository.AddAsync(webhookEvent);
            await webhookEventRepository.SaveChangesAsync();
        }

        try
        {
            await DispatchAsync(stripeEvent);
            webhookEvent.MarkAsProcessed();
            webhookEventRepository.Update(webhookEvent);
            await webhookEventRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            webhookEvent.RecordFailedAttempt(ex.Message);
            webhookEventRepository.Update(webhookEvent);
            await webhookEventRepository.SaveChangesAsync();
            logger.LogError(ex, "Failed processing Stripe webhook {EventType} ({EventId})", stripeEvent.Type, eventId.Value);
            throw;
        }
    }

    private async Task DispatchAsync(Event stripeEvent)
    {
        switch (stripeEvent.Type)
        {
            case EventTypes.InvoicePaymentSucceeded:
                await HandleInvoicePaymentSucceededAsync(stripeEvent);
                break;
            case EventTypes.InvoicePaymentFailed:
                await HandleInvoicePaymentFailedAsync(stripeEvent);
                break;
            case EventTypes.CustomerSubscriptionDeleted:
                await HandleSubscriptionDeletedAsync(stripeEvent);
                break;
            case EventTypes.CustomerSubscriptionUpdated:
                await HandleSubscriptionUpdatedAsync(stripeEvent);
                break;
            default:
                logger.LogInformation("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                break;
        }
    }

    private async Task HandleInvoicePaymentSucceededAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null)
        {
            logger.LogWarning("Stripe payload could not be deserialized as Invoice for event {EventId}", stripeEvent.Id);
            return;
        }

        var firstLine = invoice.Lines?.Data?.FirstOrDefault();
        var stripeSubscriptionId = firstLine?.SubscriptionId ?? string.Empty;
        var periodStart = firstLine?.Period?.Start;
        var periodEnd = firstLine?.Period?.End;
        var billingCycle = ResolveBillingCycle(periodStart, periodEnd);

        if (invoice.BillingReason == "subscription_create")
        {
            await subscriptionCommandService.Handle(new ActivateSubscriptionCommand(
                invoice.CustomerId ?? string.Empty,
                stripeSubscriptionId,
                invoice.Id ?? string.Empty,
                billingCycle,
                (int)invoice.AmountPaid,
                invoice.Currency ?? "usd",
                periodStart,
                periodEnd));
            return;
        }

        if (invoice.BillingReason == "subscription_cycle")
        {
            await subscriptionCommandService.Handle(new RecordSuccessfulRenewalCommand(
                stripeSubscriptionId,
                invoice.Id ?? string.Empty,
                (int)invoice.AmountPaid,
                invoice.Currency ?? "usd",
                periodStart,
                periodEnd));
        }
    }

    private async Task HandleInvoicePaymentFailedAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        if (invoice == null)
        {
            logger.LogWarning("Stripe payload could not be deserialized as Invoice for event {EventId}", stripeEvent.Id);
            return;
        }

        var stripeSubscriptionId = invoice.Lines?.Data?.FirstOrDefault()?.SubscriptionId ?? string.Empty;

        await subscriptionCommandService.Handle(new StartGracePeriodCommand(
            stripeSubscriptionId,
            invoice.Id ?? string.Empty,
            (int)invoice.AmountDue,
            invoice.Currency ?? "usd",
            DateTime.UtcNow));
    }

    private async Task HandleSubscriptionDeletedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null)
        {
            logger.LogWarning("Stripe payload could not be deserialized as Subscription for event {EventId}", stripeEvent.Id);
            return;
        }

        var reason = subscription.CancellationDetails?.Reason == "payment_failed"
            ? "PAYMENT_FAILURE"
            : "VOLUNTARY_CANCELLATION";

        await subscriptionCommandService.Handle(new DegradeSubscriptionCommand(
            subscription.Id,
            reason,
            DateTime.UtcNow));
    }

    private async Task HandleSubscriptionUpdatedAsync(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;
        if (subscription == null)
        {
            logger.LogWarning("Stripe payload could not be deserialized as Subscription for event {EventId}", stripeEvent.Id);
            return;
        }

        var firstItem = subscription.Items?.Data?.FirstOrDefault();
        var newBillingCycle = firstItem?.Price?.Recurring?.Interval == "month"
            ? "MONTHLY"
            : "ANNUAL";

        await subscriptionCommandService.Handle(new UpdateBillingCycleCommand(
            subscription.Id,
            newBillingCycle,
            firstItem?.CurrentPeriodStart,
            firstItem?.CurrentPeriodEnd));
    }

    private static string ResolveBillingCycle(DateTime? periodStart, DateTime? periodEnd)
    {
        if (!periodStart.HasValue || !periodEnd.HasValue)
            return "MONTHLY";

        var days = (periodEnd.Value - periodStart.Value).TotalDays;
        return days > 45 ? "ANNUAL" : "MONTHLY";
    }
}

public static class EventTypes
{
    public const string CustomerSubscriptionUpdated = "customer.subscription.updated";
    public const string CustomerSubscriptionDeleted = "customer.subscription.deleted";
    public const string InvoicePaymentSucceeded = "invoice.payment_succeeded";
    public const string InvoicePaymentFailed = "invoice.payment_failed";
}