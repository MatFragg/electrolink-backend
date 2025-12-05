using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

/// <summary>
/// <para>Implementation of <see cref="IPaymentTransactionQueryService"/>.</para>
/// </summary>
public class PaymentTransactionQueryService(IPaymentTransactionRepository paymentTransactionRepository) : IPaymentTransactionQueryService{

    /// <inheritdoc/>
    public async Task<IEnumerable<PaymentTransaction>> Handle(GetPaymentTransactionsBySubscriptionIdQuery query)
    {
        return await paymentTransactionRepository.ListBySubscriptionIdAsync(query.SubscriptionId);
    }
}   