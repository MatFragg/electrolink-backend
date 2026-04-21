namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Record successful renewal payment.
/// Webhook: invoice.payment_succeeded (billing_reason: subscription_cycle)
/// </summary>
public record RecordSuccessfulRenewalCommand(
    string StripeSubscriptionId,
    string StripeInvoiceId,
    int AmountPaid,
    string Currency,
    DateTime NewPeriodStart,
    DateTime NewPeriodEnd);
