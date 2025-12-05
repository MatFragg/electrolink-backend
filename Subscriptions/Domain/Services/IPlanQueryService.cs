using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for plan query services.</para>
/// <para>Handles retrieving plan data, returning full aggregates.</para>
/// </summary>
public interface IPlanQueryService
{
    /// <summary>
    /// <para>Handles the query to get a plan by its ID.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetPlanByIdQuery"/>.</param>
    /// <returns>The <see cref="Plan"/> if found, otherwise null.</returns>
    Task<Plan?> Handle(GetPlanByIdQuery query);

    /// <summary>
    /// <para>Handles the query to get all plans.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetAllPlansQuery"/>.</param>
    /// <returns>An enumerable of <see cref="Plan"/> objects.</returns>
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query);

    /// <summary>
    /// <para>Handles the query to get the default plan for a specific user role.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetDefaultPlanByRoleQuery"/>.</param>
    /// <returns>The default <see cref="Plan"/> for the role, or null if not found.</returns>
    Task<Plan?> Handle(GetDefaultPlanByRoleQuery query);

    /// <summary>
    /// <para>Handles the query to get all plans for a specific user role.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetPlansByRoleQuery"/>.</param>
    /// <returns>An enumerable of <see cref="Plan"/> objects.</returns>
    Task<IEnumerable<Plan>> Handle(GetPlansByRoleQuery query);

    /// <summary>
    /// <para>Handles the query to get the default plan.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetDefaultPlanQuery"/>.</param>
    /// <returns>The default <see cref="Plan"/>, or null if not found .</returns>
    Task<Plan?> Handle(GetDefaultPlanQuery query);
    
    /// <summary>
    /// <para>Handles the query to get detailed information about a specific plan.</para>
    /// </summary>
    /// <param name="query">The <see cref="GetPlanDetailsQuery"/>.</param>
    /// <returns>The <see cref="Plan"/> with detailed information, or null if not found.</returns>
    Task<Plan?> Handle(GetPlanDetailsQuery query);

}