namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public enum EPaymentStatus
{
    Succeeded = 1,
    Failed = 2,
    Refunded = 3
}

public record PaymentStatus
{
    public EPaymentStatus Value { get; }

    private PaymentStatus(EPaymentStatus value) => Value = value;

    public static PaymentStatus Succeeded => new(EPaymentStatus.Succeeded);
    public static PaymentStatus Failed => new(EPaymentStatus.Failed);
    public static PaymentStatus Refunded => new(EPaymentStatus.Refunded);

    public static PaymentStatus From(string value) => value.ToUpperInvariant() switch
    {
        "SUCCEEDED" => Succeeded,
        "FAILED" => Failed,
        "REFUNDED" => Refunded,
        _ => throw new ArgumentException($"Invalid PaymentStatus: {value}")
    };

    public override string ToString() => Value switch
    {
        EPaymentStatus.Succeeded => "SUCCEEDED",
        EPaymentStatus.Failed => "FAILED",
        EPaymentStatus.Refunded => "REFUNDED",
        _ => Value.ToString().ToUpperInvariant()
    };
}
