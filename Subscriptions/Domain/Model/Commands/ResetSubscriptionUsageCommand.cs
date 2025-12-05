using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to reset the usage counter for a subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription.</param>
public record ResetSubscriptionUsageCommand(Guid SubscriptionId);