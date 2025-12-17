using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;


/// <summary>
/// Repository interface for managing <see cref="PaymentTransaction"/> aggregates.
/// </summary>
public interface IPaymentTransactionRepository : IBaseRepository<PaymentTransaction, Guid>
{
    /// <summary>
    /// Lists all payment transactions for a specific subscription.
    /// </summary>
    /// <param name="subscriptionId">The ID of the subscription.</param>
    /// <returns>An enumerable of <see cref="PaymentTransaction"/> objects.</returns>
    Task<IEnumerable<PaymentTransaction>> ListBySubscriptionIdAsync(Guid subscriptionId);

}