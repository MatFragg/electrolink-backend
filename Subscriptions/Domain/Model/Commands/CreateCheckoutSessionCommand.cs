namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// LEGACY - Compatibility command. Not used in tactical design.
/// </summary>
public record CreateCheckoutSessionCommand(string SubscriptionId, string BillingCycle);
