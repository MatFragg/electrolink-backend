using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;

public class StripeSettings
{
    public const string SectionName = "Stripe";

    [Required(ErrorMessage = "Stripe SecretKey is required")]
    public string SecretKey { get; set; } = string.Empty;

    public string? PublishableKey { get; set; }

    [Required(ErrorMessage = "Stripe WebhookSecret is required")]
    public string WebhookSecret { get; set; } = string.Empty;
}
