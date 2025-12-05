namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for changing a subscription's plan.
/// </summary>
/// <param name="NewPlanId">The ID of the new plan.</param>
/// <param name="NewEndDate">The new end date for the subscription.</param>
/// <param name="StripeSubscriptionId">The new Stripe subscription ID (if changed with the plan).</param>
public record ChangeSubscriptionPlanResource(Guid NewPlanId, DateTime NewEndDate, string StripeSubscriptionId);