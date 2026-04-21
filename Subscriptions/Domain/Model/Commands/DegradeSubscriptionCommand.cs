namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Degrade subscription from PREMIUM to BASIC.
/// Triggered by: Grace period expiration job OR voluntary cancellation at period end.
/// </summary>
public record DegradeSubscriptionCommand(
    string StripeSubscriptionId,
    string Reason,  // "PAYMENT_FAILURE" | "VOLUNTARY_CANCELLATION"
    DateTime DegradedAt);
