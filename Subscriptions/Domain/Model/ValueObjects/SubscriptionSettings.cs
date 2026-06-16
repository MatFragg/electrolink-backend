namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record SubscriptionSettings
{
    public string SuccessUrl { get; init; } = string.Empty;
    public string CancelUrl { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
}
