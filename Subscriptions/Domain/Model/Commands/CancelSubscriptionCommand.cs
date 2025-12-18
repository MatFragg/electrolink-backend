using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to cancel a subscription in Stripe.
/// </summary>
public record CancelSubscriptionInGatewayCommand(
    SubscriptionId SubscriptionId,
    bool Immediately = false // false = al final del período
);