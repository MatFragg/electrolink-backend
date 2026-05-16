namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public class SubscriptionSettings
{
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
