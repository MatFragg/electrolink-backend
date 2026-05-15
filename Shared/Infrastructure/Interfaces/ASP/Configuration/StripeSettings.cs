namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;

public class StripeSettings
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrEmpty(SecretKey))
            throw new InvalidOperationException("Stripe SecretKey is required.");
    }
}