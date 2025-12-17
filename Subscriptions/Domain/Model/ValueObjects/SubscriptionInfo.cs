namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;


/// <summary>
/// Provider-agnostic DTO representing subscription information from payment gateway.
/// </summary>
public record SubscriptionInfo(string subscriptionId,
    string customerId,
    string priceId,
    string status,
    DateTime startDate,
    DateTime currentPeriodEnd,
    DateTime? trialEnd,
    DateTime? canceledAt,
    DateTime? cancelAt,
    bool cancelAtPeriodEnd,
    decimal amount,
    string currency);