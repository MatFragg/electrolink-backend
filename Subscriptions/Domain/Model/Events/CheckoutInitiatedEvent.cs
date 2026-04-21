using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

/// <summary>
/// Domain event: User initiated checkout to upgrade from BASIC to PREMIUM.
/// Stripe checkout session created.
/// 
/// Consumed by: Notifications BC (send checkout link via email)
/// </summary>
public record CheckoutInitiatedEvent(
    string SubscriptionId,
    string UserId,
    string BusinessRole,
    string BillingCycle,
    string StripeCheckoutSessionId,
    DateTime InitiatedAt) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = InitiatedAt;
}
