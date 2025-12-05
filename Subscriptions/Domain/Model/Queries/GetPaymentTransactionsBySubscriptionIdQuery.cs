namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query to get payment transactions for a specific subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription.</param>
public record GetPaymentTransactionsBySubscriptionIdQuery(Guid SubscriptionId);