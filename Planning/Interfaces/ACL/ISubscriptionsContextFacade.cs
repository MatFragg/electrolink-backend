using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.ACL;

/// <summary>
/// Facade interface for accessing Subscriptions bounded context from Planning.
/// Provides read-only operations to query subscription plan details and limits.
/// </summary>
public interface ISubscriptionsContextFacade
{
    /// <summary>
    /// Get the subscription plan details for a specific client.
    /// </summary>
    /// <param name="clientId">The client's ID</param>
    /// <returns>Plan information including monthly request limit, or null if no active subscription</returns>
    Task<SubscriptionPlanInfo?> GetSubscriptionPlanByClientIdAsync(ClientId clientId);

    /// <summary>
    /// Check if a client has reached their monthly request limit.
    /// </summary>
    /// <param name="clientId">The client's ID</param>
    /// <param name="year">Year to check</param>
    /// <param name="month">Month to check</param>
    /// <returns>True if limit is reached, false otherwise</returns>
    Task<bool> IsMonthlyLimitReachedAsync(ClientId clientId, int year, int month);
}

/// <summary>
/// DTO representing subscription plan information exposed to Planning BC
/// </summary>
public record SubscriptionPlanInfo(
    string PlanName,
    int MonthlyRequestLimit,
    bool IsBasicPlan);

