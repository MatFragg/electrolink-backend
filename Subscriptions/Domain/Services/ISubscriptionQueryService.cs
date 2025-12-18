using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for subscription query services.</para>
/// <para>Handles retrieving subscription data, returning full aggregates.</para>
/// </summary>
public interface ISubscriptionQueryService
{
    /// <summary>
    /// <para>Handles the query to get a subscription by its ID.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetSubscriptionByIdQuery"/>.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);

    /// <summary>
    /// <para>Handles the query to get a subscription by user ID.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetSubscriptionByUserIdQuery"/>.</param>
    /// <returns>The <see cref="Subscription"/> if found, otherwise null.</returns>
    Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query);

    /// <summary>
    /// <para>Handles the query to get all active subscriptions.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetAllActiveSubscriptionsQuery"/>.</param>
    /// <returns>An enumerable of active <see cref="Subscription"/> objects.</returns>
    Task<IEnumerable<Subscription>> Handle(GetAllActiveSubscriptionsQuery query);

    /// <summary>
    /// <para>Handles the query to get a user's specific plan benefit.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetUserBenefitQuery"/>.</param>
    /// <returns>The <see cref="Benefit"/> if found for the user, otherwise null.</returns>
    Task<Benefit?> Handle(GetUserBenefitQuery query);

    /// <summary>
    /// <para>Handles the query to get a subscription by payment gateway subscription ID.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetLocalSubscriptionIdQuery"/>.</param>
    /// <returns>The <see cref="Guid"/> if found, otherwise null.</returns> 
    Task<Guid?> Handle(GetLocalSubscriptionIdQuery query);
}