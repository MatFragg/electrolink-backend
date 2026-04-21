using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Payment failed, entering 7-day grace period.
/// Webhook: invoice.payment_failed
/// 
/// Consumed by: 
///   - Notifications BC: Send payment failure alert, suggest retry
///   - Monitoring BC: Audit trail, track at-risk subscriptions
/// </summary>
public record GracePeriodStartedEvent(
    string SubscriptionId,
    string UserId,
    string BusinessRole,
    string PlanType,
    string StripeInvoiceId,
    DateTime GracePeriodEndsAt,
    DateTime FailedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = FailedAt;
}
