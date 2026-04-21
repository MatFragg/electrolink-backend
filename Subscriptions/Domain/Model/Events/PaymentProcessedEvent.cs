using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Payment processed successfully (renewal or recovery from grace period).
/// Webhook: invoice.payment_succeeded (billing_reason: subscription_cycle)
/// 
/// Consumed by: Notifications BC (send renewal confirmation), Monitoring BC (audit)
/// </summary>
public record PaymentProcessedEvent(
    string SubscriptionId,
    string UserId,
    string StripeInvoiceId,
    DateTime NewPeriodEnd,
    bool WasInGracePeriod,
    DateTime ProcessedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ProcessedAt;
}
