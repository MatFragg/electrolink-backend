using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to process a payment (typically from a webhook or scheduled job).
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription associated with the payment.</param>
/// <param name="Amount">The amount of the payment.</param>
/// <param name="Currency">The currency of the payment.</param>
/// <param name="TransactionDate">The date and time of the transaction.</param>
/// <param name="Status">The status of the payment.</param>
/// <param name="GatewayTransactionId">The payment gateway's transaction ID.</param>
/// <param name="Message">Optional message.</param>
public record ProcessPaymentCommand(
    SubscriptionId   SubscriptionId,
    decimal? Amount,
    string Currency,
    DateTime TransactionDate,
    EPaymentStatus Status,
    string GatewayTransactionId,
    string Message = ""
): IRequest<Unit>;