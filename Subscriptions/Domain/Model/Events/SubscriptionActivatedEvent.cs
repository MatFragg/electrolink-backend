using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: Subscription activated (BASIC → PREMIUM).
/// Webhook: invoice.payment_succeeded (billing_reason: subscription_create)
/// 
/// Consumed by: 
///   - Planning BC: Create/reactivate ServiceCatalog for TECHNICIAN
///   - Notifications BC: Send activation confirmation email
///   - Monitoring BC: Audit trail
/// </summary>
public record SubscriptionActivatedEvent(
    string SubscriptionId,
    string UserId,
    string BusinessRole,
    string PlanType,
    string BillingCycle,
    string StripeSubscriptionId,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    DateTime ActivatedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ActivatedAt;
}
