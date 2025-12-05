using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;


/// <summary>
/// Repository interface for managing <see cref="PaymentTransaction"/> aggregates.
/// </summary>
public interface IPaymentTransactionRepository : IBaseRepository<PaymentTransaction>
{
    
    /// <summary>
    /// Finds a payment transaction by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The transaction ID (Guid).</param>
    /// <returns>The <see cref="PaymentTransaction"/> if found, otherwise null.</returns>
    new Task<PaymentTransaction?> FindByIdAsync(Guid transactionId); // Uses FindByGuidAsync from BaseRepository
    /// <summary>
    /// Lists all payment transactions for a specific subscription.
    /// </summary>
    /// <param name="subscriptionId">The ID of the subscription.</param>
    /// <returns>An enumerable of <see cref="PaymentTransaction"/> objects.</returns>
    Task<IEnumerable<PaymentTransaction>> ListBySubscriptionIdAsync(Guid subscriptionId);

}