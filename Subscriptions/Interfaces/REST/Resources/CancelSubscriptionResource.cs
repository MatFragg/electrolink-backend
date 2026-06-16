using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record CancelSubscriptionResource(
    [property: Required] string Reason,
    string? Feedback
);
