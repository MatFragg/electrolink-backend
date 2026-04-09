namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record SubmitReviewResource(
    int Rating,
    string? Comment,
    Dictionary<string, int> Categories
);