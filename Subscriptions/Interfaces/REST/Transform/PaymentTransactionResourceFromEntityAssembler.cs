using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="PaymentTransaction"/> entity to <see cref="PaymentTransactionResource"/>.
/// </summary>
public static class PaymentTransactionResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a <see cref="PaymentTransaction"/> aggregate to a <see cref="PaymentTransactionResource"/>.
    /// </summary>
    /// <param name="transaction">The payment transaction aggregate to convert.</param>
    /// <returns>The created payment transaction resource.</returns>
    public static PaymentTransactionResource ToResource(PaymentTransaction transaction)
    {
        return new PaymentTransactionResource(
            transaction.Id,
            transaction.SubscriptionId.Value,
            transaction.Amount,
            transaction.Currency,
            transaction.TransactionDate,
            transaction.Status.ToString(),
            transaction.GatewayTransactionId,
            transaction.Message
        );
    }
}