using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for payment transaction query services.</para>
/// <para>Handles retrieving payment transaction data, returning full aggregates.</para>
/// </summary>
public interface IPaymentTransactionQueryService
{
    /// <summary>
    /// <para>Handles the query to get payment transactions for a specific subscription.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetPaymentTransactionsBySubscriptionIdQuery"/>.</param>
    /// <returns>An enumerable of <see cref="PaymentTransaction"/> objects.</returns>
    Task<IEnumerable<PaymentTransaction>> Handle(GetPaymentTransactionsBySubscriptionIdQuery query);
}