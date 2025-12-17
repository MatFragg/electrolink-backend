using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;


/// <summary>
/// Repository interface for managing <see cref="Subscription"/> aggregates.
/// </summary>
public interface ISubscriptionRepository : IBaseRepository<Subscription, SubscriptionId>
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
    /// <param name="gatewayCustomerId">The Payment Gateway Customer ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindByPaymentGatewayCustomerIdAsync(PaymentGatewayCustomerId gatewayCustomerId); 

    /// <summary>
    /// Finds a subscription by its Stripe Subscription ID.
    /// </summary>
    /// <param name="gatewaySubscriptionId">The Payment Gateway Subscription ID.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindByPaymentGatewaySubscriptionIdAsync(PaymentGatewaySubscriptionId gatewaySubscriptionId); 
    
    /// <summary>
    /// Finds an active subscription by the user's unique identifier.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The active <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> FindActiveByUserIdAsync(UserId userId);
    
}   