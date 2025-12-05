using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to cancel a subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription to cancel.</param>
/// <param name="CancellationEffectiveDate">The date when the cancellation becomes effective.</param>
public record CancelSubscriptionCommand(Guid SubscriptionId, DateTime CancellationEffectiveDate);