namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for representing a payment transaction in API responses.
/// </summary>
/// <param name="Id">Unique identifier for the payment transaction.</param>
/// <param name="SubscriptionId">The ID of the subscription associated with this transaction.</param>
/// <param name="Amount">The amount of the transaction.</param>
/// <param name="Currency">The currency of the transaction.</param>
/// <param name="TransactionDate">The date and time when the transaction occurred.</param>
/// <param name="Status">The status of the transaction.</param>
/// <param name="GatewayTransactionId">The identifier from the payment gateway.</param>
/// <param name="Message">Optional message or reason for the transaction status.</param>
public record PaymentTransactionResource(
    Guid Id,
    Guid SubscriptionId,
    decimal Amount,
    string Currency,
    DateTime TransactionDate,
    string Status,
    string GatewayTransactionId,
    string Message
);