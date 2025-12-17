using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to change the plan of an existing subscription.
/// </summary>
/// <param name="SubscriptionId">The ID of the subscription to change.</param>
/// <param name="NewPlanId">The ID of the new plan.</param>
/// <param name="NewEndDate">The new end date for the subscription.</param>
/// <param name="GatewaySubscriptionId">The new Stripe subscription ID (if changed).</param>
public record ChangeSubscriptionPlanCommand(SubscriptionId SubscriptionId, Guid NewPlanId, DateTime NewEndDate, PaymentGatewaySubscriptionId GatewaySubscriptionId);