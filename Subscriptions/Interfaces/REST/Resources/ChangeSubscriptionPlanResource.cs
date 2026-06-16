namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Resource to change a subscription's plan.
/// </summary>
public record ChangeSubscriptionPlanResource(
    [property: Required] string NewPriceId,
    string ProrationBehavior = "create_prorations" // create_prorations, none, always_invoice
);