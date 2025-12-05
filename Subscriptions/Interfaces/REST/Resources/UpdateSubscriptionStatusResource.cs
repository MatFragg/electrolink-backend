namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for updating a subscription's status.
/// </summary>
/// <param name="NewStatus">The new status for the subscription (e.g., "Active", "Paused").</param>
public record UpdateSubscriptionStatusResource(string NewStatus);