namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record TechnicalPreferences(
    int MaxRequestsPerDay,
    bool AllowUrgentRequests,
    List<string> ServiceTagsAccepted
);