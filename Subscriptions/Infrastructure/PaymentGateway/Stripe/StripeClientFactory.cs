using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

/// <summary>
/// Factory to create services of the Stripe SDK with centralized configuration.
/// </summary>
public class StripeClientFactory
{
    private readonly StripeConfiguration _config;

    public StripeClientFactory(StripeConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _config.Validate();
        
        // Configurar API Key globalmente
        StripeConfiguration.ApiKey = _config.SecretKey;
    }

    public CustomerService CreateCustomerService() => new CustomerService();
    public SubscriptionService CreateSubscriptionService() => new SubscriptionService();
    public PriceService CreatePriceService() => new PriceService();
    public InvoiceService CreateInvoiceService() => new InvoiceService();
    public PaymentIntentService CreatePaymentIntentService() => new PaymentIntentService();
    public BillingPortal.SessionService CreateBillingPortalSessionService() => new BillingPortal.SessionService();
    public Checkout.SessionService CreateCheckoutSessionService() => new Checkout.SessionService();
}