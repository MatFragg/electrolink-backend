namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum representing the reason for subscription degradation.
/// </summary>
public enum EDegradationReason
{
    PaymentFailure,
    VoluntaryCancellation
}

/// <summary>
/// Value Object representing the reason for subscription degradation.
/// </summary>
public record DegradationReason
{
    public EDegradationReason Value { get; }

    private DegradationReason(EDegradationReason value) => Value = value;

    /// <summary>
    /// Factory: Degradation due to payment failure (grace period expired).
    /// </summary>
    public static DegradationReason PaymentFailure => new(EDegradationReason.PaymentFailure);

    /// <summary>
    /// Factory: Degradation due to voluntary cancellation at period end.
    /// </summary>
    public static DegradationReason VoluntaryCancellation => new(EDegradationReason.VoluntaryCancellation);

    /// <summary>
    /// Parse from string representation.
    /// </summary>
    public static DegradationReason From(string value) => value.ToUpperInvariant() switch
    {
        "PAYMENT_FAILURE" => PaymentFailure,
        "VOLUNTARY_CANCELLATION" => VoluntaryCancellation,
        _ => throw new ArgumentException($"Invalid DegradationReason: {value}")
    };

    /// <summary>
    /// String representation in uppercase with underscores.
    /// </summary>
    public override string ToString() => Value switch
    {
        EDegradationReason.PaymentFailure => "PAYMENT_FAILURE",
        EDegradationReason.VoluntaryCancellation => "VOLUNTARY_CANCELLATION",
        _ => Value.ToString()
    };
}

