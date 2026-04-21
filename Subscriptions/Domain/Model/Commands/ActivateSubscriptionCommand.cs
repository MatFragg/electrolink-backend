namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Activate subscription (BASIC → PREMIUM).
/// Webhook: invoice.payment_succeeded (billing_reason: subscription_create)
/// </summary>
public record ActivateSubscriptionCommand(
    string StripeCustomerId,
    string StripeSubscriptionId,
    string StripeInvoiceId,
    string BillingCycle,
    int AmountPaid,
    string Currency,
    DateTime PeriodStart,
    DateTime PeriodEnd);
