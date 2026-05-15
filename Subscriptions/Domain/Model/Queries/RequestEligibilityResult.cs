namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query response model for request eligibility checks.
/// </summary>
public record RequestEligibilityResult(
    bool CanRequest,
    bool IsPriorityAllowed,
    int? RemainingRequests,
    string PlanType,

    bool UpgradeRequired
);
