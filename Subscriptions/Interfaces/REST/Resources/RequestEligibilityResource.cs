namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record RequestEligibilityResource(
    bool CanRequest,
    bool IsPriorityAllowed,
    int? RemainingRequests,
    string PlanType,
    bool UpgradeRequired,
    string? UpgradePromptMessage);

