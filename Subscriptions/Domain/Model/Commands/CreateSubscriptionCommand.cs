using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using UserId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.UserId;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to create a new subscription.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
/// <param name="PlanId">The ID of the plan.</param>
/// <param name="StartDate">The start date of the subscription.</param>
/// <param name="EndDate">The end date of the subscription.</param>
/// <param name="GatewayCustomerId">Stripe's customer ID.</param>
/// <param name="GatewaySubscriptionId">Stripe's subscription ID.</param>
/// <param name="InitialStatus">The initial status of the subscription.</param>
/// <param name="TrialEndsAt">Optional trial end date.</param>
public record CreateSubscriptionCommand(
    UserId UserId,
    Guid PlanId,
    DateTime StartDate,
    DateTime EndDate,
    PaymentGatewayCustomerId GatewayCustomerId,
    PaymentGatewaySubscriptionId GatewaySubscriptionId,
    ESubscriptionStatus InitialStatus,
    DateTime? TrialEndsAt = null
);