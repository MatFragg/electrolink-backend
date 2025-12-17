using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to change the plan of a subscription in Stripe.
/// </summary>
public record ChangeSubscriptionPlanInGatewayCommand(
    SubscriptionId SubscriptionId,
    PlanId NewPlanId,
    string ProrationBehavior = "create_prorations" // create_prorations, none, always_invoice
);