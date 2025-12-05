namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;


/// <summary>
/// Query to get a subscription by its ID.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription.</param>
public record GetSubscriptionByIdQuery(Guid SubscriptionId);