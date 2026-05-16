namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum representing the status of a subscription in the tactical design.
/// </summary>
public enum ESubscriptionStatus
{
    Active,
    GracePeriod,
    CancelledPending,
    Degraded,
    PendingInstallation,
    CancelledRefunded
}

/// <summary>
/// Value Object representing the current status of a subscription.
/// </summary>
public record SubscriptionStatus
{
    public ESubscriptionStatus Value { get; }

    private SubscriptionStatus(ESubscriptionStatus value) => Value = value;

    /// <summary>
    /// Active subscription with all benefits available.
    /// </summary>
    public static SubscriptionStatus Active => new(ESubscriptionStatus.Active);

    /// <summary>
    /// Payment failed, in 7-day grace period before degradation.
    /// </summary>
    public static SubscriptionStatus GracePeriod => new(ESubscriptionStatus.GracePeriod);

    /// <summary>
    /// Voluntary cancellation scheduled for period end.
    /// </summary>
    public static SubscriptionStatus CancelledPending => new(ESubscriptionStatus.CancelledPending);

    /// <summary>
    /// Degraded to BASIC plan due to payment failure or voluntary cancellation.
    /// </summary>
    public static SubscriptionStatus Degraded => new(ESubscriptionStatus.Degraded);

    /// <summary>
    /// Enterprise subscription paid, awaiting IoT device installation.
    /// </summary>
    public static SubscriptionStatus PendingInstallation => new(ESubscriptionStatus.PendingInstallation);

    /// <summary>
    /// Enterprise subscription cancelled with refund.
    /// </summary>
    public static SubscriptionStatus CancelledRefunded => new(ESubscriptionStatus.CancelledRefunded);

    /// <summary>
    /// Parse from string representation.
    /// </summary>
    public static SubscriptionStatus From(string value) => value.ToUpperInvariant() switch
    {
        "ACTIVE" => Active,
        "GRACE_PERIOD" => GracePeriod,
        "CANCELLED_PENDING" => CancelledPending,
        "DEGRADED" => Degraded,
        "PENDING_INSTALLATION" => PendingInstallation,
        "CANCELLED_REFUNDED" => CancelledRefunded,
        _ => throw new ArgumentException($"Invalid SubscriptionStatus: {value}")
    };

    /// <summary>
    /// Check if status is Active.
    /// </summary>
    public bool IsActive => Value == ESubscriptionStatus.Active;

    /// <summary>
    /// Check if status is in grace period.
    /// </summary>
    public bool IsInGracePeriod => Value == ESubscriptionStatus.GracePeriod;

    /// <summary>
    /// Check if cancellation is pending.
    /// </summary>
    public bool IsCancelledPending => Value == ESubscriptionStatus.CancelledPending;

    /// <summary>
    /// Check if subscription is degraded.
    /// </summary>
    public bool IsDegraded => Value == ESubscriptionStatus.Degraded;

    /// <summary>
    /// Check if awaiting installation.
    /// </summary>
    public bool IsPendingInstallation => Value == ESubscriptionStatus.PendingInstallation;

    /// <summary>
    /// Check if cancelled with refund.
    /// </summary>
    public bool IsCancelledRefunded => Value == ESubscriptionStatus.CancelledRefunded;

    /// <summary>
    /// String representation in uppercase with underscores.
    /// </summary>
    public override string ToString() => Value switch
    {
        ESubscriptionStatus.Active => "ACTIVE",
        ESubscriptionStatus.GracePeriod => "GRACE_PERIOD",
        ESubscriptionStatus.CancelledPending => "CANCELLED_PENDING",
        ESubscriptionStatus.Degraded => "DEGRADED",
        ESubscriptionStatus.PendingInstallation => "PENDING_INSTALLATION",
        ESubscriptionStatus.CancelledRefunded => "CANCELLED_REFUNDED",
        _ => Value.ToString()
    };
}

