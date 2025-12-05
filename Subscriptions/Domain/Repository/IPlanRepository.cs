using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

/// <summary>
/// Repository interface for managing <see cref="Plan"/> aggregates.
/// </summary>
public interface IPlanRepository : IBaseRepository<Plan>
{
    /// <summary>
    /// Finds a plan by its unique identifier.
    /// </summary>
    /// <param name="planId">The plan ID.</param>
    /// <returns>The <see cref="Plan"/> if found, otherwise null.</returns>
    Task<Plan?> FindByIdAsync(PlanId planId);
    
    /// <summary>
    /// Finds the default plan.
    /// </summary>
    /// <returns></returns>
    Task<Plan?> FindDefaultAsync();
    
    /// <summary>
    /// Finds the default plan for a specific user role.
    /// </summary>
    /// <param name="role">The target user role.</param>
    /// <returns>The default <see cref="Plan"/> for the role, or null if not found.</returns>
    Task<Plan?> FindDefaultPlanByRoleAsync(EUserRole role);
    
    
    /// <summary>
    /// <para>Lists all plans targeting a specific user role.</para>
    /// </summary>
    /// <param name="role">The target user role.</param>
    /// <returns>An enumerable of <see cref="Plan"/> objects for the specified role.</returns>
    Task<IEnumerable<Plan>> ListPlansByRoleAsync(EUserRole role);
    
    /// <summary>
    /// Finds a plan by its Stripe Price ID.
    /// </summary>
    /// <param name="stripePriceId">The Stripe Price ID.</param>
    /// <returns>The <see cref="Plan"/> if found, otherwise null.</returns>
    Task<Plan?> FindByStripePriceIdAsync(string stripePriceId);
}