using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record InitiateCheckoutResource(
    [Required] string PlanType,
    [Required] string BillingCycle,
    [Required, Url] string SuccessUrl,
    [Required, Url] string CancelUrl);

