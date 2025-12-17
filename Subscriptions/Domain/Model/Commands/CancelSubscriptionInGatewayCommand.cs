namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to cancel a subscription in Stripe.
/// </summary>
public record CancelSubscriptionInStripeCommand(
    Guid SubscriptionId,
    bool Immediately = false // false = al final del período
);