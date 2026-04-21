namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Start 7-day grace period.
/// Webhook: invoice.payment_failed
/// </summary>
public record StartGracePeriodCommand(
    string StripeSubscriptionId,
    string StripeInvoiceId,
    int AmountDue,
    string Currency,
    DateTime FailedAt);
