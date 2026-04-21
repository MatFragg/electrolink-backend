namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

public record RequestEligibilityResult(
    bool CanRequest,
    bool IsPriorityAllowed,
    int? RemainingRequests,
    string PlanType,
    bool UpgradeRequired);

