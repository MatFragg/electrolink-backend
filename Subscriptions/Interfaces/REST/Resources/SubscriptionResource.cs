namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for representing a subscription in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the subscription.</param>
/// <param name="UserId">The ID of the user.</param>
/// <param name="PlanId">The ID of the plan.</param>
/// <param name="Status">The current status of the subscription.</param>
/// <param name="StartDate">The date when the subscription started.</param>
/// <param name="EndDate">The date when the current subscription period is scheduled to end.</param>
/// <param name="CurrentUsage">Current usage counter for limited benefits.</param>
/// <param name="IsPremium">Indicates if the subscription grants premium access.</param>
/// <param name="IsCertified">Indicates if the subscription grants certification status.</param>
/// <param name="CanUseBoost">Indicates if the subscription allows using the boost feature.</param>
public record SubscriptionResource(
    Guid Id,
    int UserId,
    Guid PlanId,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    int CurrentUsage,
    bool IsPremium,
    bool IsCertified,
    bool CanUseBoost
);