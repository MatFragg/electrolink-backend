using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to apply a discount or promotional code.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription to apply the discount to.</param>
/// <param name="DiscountCode">The promotional code to apply.</param>
/// <param name="AppliedByUserId">The ID of the user applying the discount (if admin).</param>
public record ApplyDiscountCommand(Guid SubscriptionId, string DiscountCode, Guid? AppliedByUserId = null);