namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource to change a subscription's plan.
/// </summary>
public record ChangeSubscriptionPlanResource(
    string NewPriceId,
    string ProrationBehavior = "create_prorations" // create_prorations, none, always_invoice
);