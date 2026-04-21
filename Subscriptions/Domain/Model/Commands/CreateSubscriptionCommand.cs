namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Create subscription for new user.
/// Called by InitializeSubscriptionForNewUser policy when ProfileCompleted event is received.
/// </summary>
public record CreateSubscriptionCommand(
    string UserId,
    string BusinessRole);  // "TECHNICIAN" | "HOMEOWNER"
