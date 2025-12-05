namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to synchronize a subscription from Stripe to our DB..
/// Used by webhooks to maintain consistency.
/// </summary>
public record SyncSubscriptionFromStripeCommand(
    string StripeSubscriptionId,
    string StripeCustomerId,
    string StripePriceId,
    string Status, // active, trialing, past_due, canceled, etc.
    DateTime CurrentPeriodStart,
    DateTime CurrentPeriodEnd,
    DateTime? TrialEnd,
    DateTime? CanceledAt,
    DateTime? CancelAt,
    bool CancelAtPeriodEnd
);