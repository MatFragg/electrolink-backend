namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record CancelSubscriptionResource(
    string Reason,
    string? Feedback
);
