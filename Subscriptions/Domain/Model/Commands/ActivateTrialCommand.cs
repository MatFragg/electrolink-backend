
namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to activate a trial period for a subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription.</param>
/// <param name="TrialEndDate">The date when the trial period ends.</param>
public record ActivateTrialCommand(Guid SubscriptionId, DateTime TrialEndDate);