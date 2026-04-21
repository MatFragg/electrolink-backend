namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// LEGACY - Compatibility type. Not used in tactical domain.
/// </summary>
public record SubscriptionInfo
{
    public string SubscriptionId { get; }
    public string UserId { get; }
    public string Status { get; }

    private SubscriptionInfo(string subscriptionId, string userId, string status)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        Status = status;
    }

    public static SubscriptionInfo From(string subscriptionId, string userId, string status) =>
        new(subscriptionId, userId, status);
}

