namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource to cancel a subscription.
/// </summary>
public record CancelSubscriptionResource(
    bool Immediately = false
);
