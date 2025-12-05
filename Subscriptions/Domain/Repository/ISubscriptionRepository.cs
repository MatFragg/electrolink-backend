using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;


/// <summary>
/// Repository interface for managing <see cref="Subscription"/> aggregates.
/// </summary>
public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    /// <summary>
    /// Finds a subscription by its unique identifier.
    /// </summary>
    /// <param name="id">The subscription ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindBySubscriptionIdAsync(SubscriptionId id);
    
    /// <summary>
    /// Finds a subscription by the user's unique identifier.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindByUserIdAsync(UserId userId);
    
    /// <summary>
    /// <para>Lists all active subscriptions.</para>
    /// </summary>
    /// <returns>An enumerable of active <see cref="Subscription"/> objects.</returns>
    Task<IEnumerable<Subscription>> ListActiveAsync();
    
    /// <summary>
    /// Finds a subscription by its Stripe Customer ID.
    /// </summary>
    /// <param name="stripeCustomerId">The Stripe Customer ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindByStripeCustomerIdAsync(string stripeCustomerId); 

    /// <summary>
    /// Finds a subscription by its Stripe Subscription ID.
    /// </summary>
    /// <param name="stripeSubscriptionId">The Stripe Subscription ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindByStripeSubscriptionIdAsync(string stripeSubscriptionId); 
}   