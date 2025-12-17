namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

/// <summary>
/// Stripe Configuration from appsettings.json.
/// </summary>
public class StripeSettings
{
    public const string SectionName = "Stripe";

    /// <summary>
    /// Stripe Secret Key (sk_test_... o sk_live_...).
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Stripe Publishable Key (pk_test_... o pk_live_...).
    /// </summary>
    public string PublishableKey { get; set; } = string.Empty;

    /// <summary>
    /// Stripe Webhook Secret to validate signatures (whsec_...).).
    /// </summary>
    public string WebhookSecret { get; set; } = string.Empty;

    /// <summary>
    /// Validates that the configuration is complete.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(SecretKey))
            throw new InvalidOperationException("Stripe:SecretKey is not configured in appsettings.json");

        if (string.IsNullOrWhiteSpace(PublishableKey))
            throw new InvalidOperationException("Stripe:PublishableKey is not configured in appsettings.json");

        if (string.IsNullOrWhiteSpace(WebhookSecret))
            throw new InvalidOperationException("Stripe:WebhookSecret is not configured in appsettings.json");
    }
}