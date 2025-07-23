namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

public record TechnicalPreferences(
    int MaxRequestsPerDay,
    bool AllowUrgentRequests,
    List<string> ServiceTagsAccepted
);