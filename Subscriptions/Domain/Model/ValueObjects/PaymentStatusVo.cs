namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum representing the status of a payment record.
/// </summary>
public enum EPaymentStatusVo
{
    Succeeded,
    Failed,
    Refunded
}

/// <summary>
/// Value Object representing the outcome of a payment attempt.
/// </summary>
public record PaymentStatus
{
    public EPaymentStatusVo Value { get; }

    private PaymentStatus(EPaymentStatusVo value) => Value = value;

    /// <summary>
    /// Payment succeeded and invoice was paid.
    /// </summary>
    public static PaymentStatus Succeeded => new(EPaymentStatusVo.Succeeded);

    /// <summary>
    /// Payment failed (will trigger grace period or degradation).
    /// </summary>
    public static PaymentStatus Failed => new(EPaymentStatusVo.Failed);

    /// <summary>
    /// Payment was refunded.
    /// </summary>
    public static PaymentStatus Refunded => new(EPaymentStatusVo.Refunded);

    /// <summary>
    /// Parse from string representation.
    /// </summary>
    public static PaymentStatus From(string value) => value.ToUpperInvariant() switch
    {
        "SUCCEEDED" => Succeeded,
        "FAILED" => Failed,
        "REFUNDED" => Refunded,
        _ => throw new ArgumentException($"Invalid PaymentStatus: {value}")
    };

    /// <summary>
    /// String representation in uppercase.
    /// </summary>
    public override string ToString() => Value switch
    {
        EPaymentStatusVo.Succeeded => "SUCCEEDED",
        EPaymentStatusVo.Failed => "FAILED",
        EPaymentStatusVo.Refunded => "REFUNDED",
        _ => Value.ToString()
    };
}

