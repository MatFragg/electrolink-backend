using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to update the usage counter for a subscription's limited benefit.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription.</param>
public record IncrementSubscriptionUsageCommand(Guid SubscriptionId);