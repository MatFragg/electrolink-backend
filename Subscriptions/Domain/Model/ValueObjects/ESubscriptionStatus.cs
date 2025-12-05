namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum for subscription statuses.
/// </summary>
public enum ESubscriptionStatus
{
    /// <summary>
    /// The subscription is currently active.
    /// </summary>
    Active,
    /// <summary>
    /// The subscription has been cancelled but is still active until the end of the period.
    /// </summary>
    Cancelled,
    /// <summary>
    /// The subscription is temporarily paused.
    /// </summary>
    Paused,
    /// <summary>
    /// The subscription is in a trial period.
    /// </summary>
    Trial,
    /// <summary>
    /// The subscription has expired due to lack of payment or trial end.
    /// </summary>
    Expired,
    /// <summary>
    /// A payment has been initiated for the subscription.
    /// </summary>
    PaymentInitiated,
    /// <summary>
    /// A payment is due, typically after a failed attempt, and re-attempts are in progress.
    /// </summary>
    PaymentDue,
    /// <summary>
    /// The subscription is awaiting confirmation or initial processing.
    /// </summary>
    Pending
}
