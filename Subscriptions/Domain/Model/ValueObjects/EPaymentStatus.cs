namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum for payment transaction statuses.
/// </summary>
public enum EPaymentStatus
{
    /// <summary>
    /// The payment was successful.
    /// </summary>
    Success,
    /// <summary>
    /// The payment failed.
    /// </summary>
    Failed,
    /// <summary>
    /// The payment is pending.
    /// </summary>
    Pending
}