namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Resource for internal plan changes (non-Stripe).
/// </summary>
public record ChangeSubscriptionPlanInternalResource(
    Guid NewPlanId,
    DateTime NewEndDate,
    [property: Required] string StripeSubscriptionId
);