using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to update the status of an existing subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription to update.</param>
/// <param name="NewStatus">The new status to set.</param>
public record UpdateSubscriptionStatusCommand(Guid SubscriptionId, ESubscriptionStatus NewStatus);