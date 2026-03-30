namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestEligibility(
    bool CanCreate,
    string? PlanType,
    int? RemainingRequests,
    bool CanMarkAsPriority,
    string? Reason);