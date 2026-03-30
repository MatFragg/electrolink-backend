namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record RequestEligibilityResource(
    bool CanCreate,
    string? PlanType,
    int? RemainingRequests,
    bool CanMarkAsPriority,
    string? Reason
);

