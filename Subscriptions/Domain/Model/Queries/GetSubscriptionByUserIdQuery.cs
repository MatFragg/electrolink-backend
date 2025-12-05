using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;


/// <summary>
/// Query to get a subscription by user ID.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
public record GetSubscriptionByUserIdQuery(UserId UserId);