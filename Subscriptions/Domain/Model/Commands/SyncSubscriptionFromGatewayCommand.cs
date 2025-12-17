using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to synchronize a subscription from Stripe to our DB..
/// Used by webhooks to maintain consistency.
/// </summary>
public record SyncSubscriptionFromGatewayCommand(
    PaymentGatewaySubscriptionId GatewaySubscriptionId,
    PaymentGatewayCustomerId GatewayCustomerId,
    PaymentGatewayPriceId GatewayPriceId,
    string Status, // active, trialing, past_due, canceled, etc.
    DateTime CurrentPeriodStart,
    DateTime CurrentPeriodEnd,
    DateTime? TrialEnd,
    DateTime? CanceledAt,
    DateTime? CancelAt,
    bool CancelAtPeriodEnd
);