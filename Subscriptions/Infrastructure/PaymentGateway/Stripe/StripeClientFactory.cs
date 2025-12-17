using Stripe;
using BillingPortal = Stripe.BillingPortal;
using Checkout = Stripe.Checkout;


namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

/// <summary>
/// Factory to create services of the Stripe SDK with centralized configuration.
/// </summary>
public class StripeClientFactory
{
    private readonly StripeSettings _settings;

    public StripeClientFactory(StripeSettings config)
    {
        _settings = config ?? throw new ArgumentNullException(nameof(config));
        _settings.Validate();
        
        // Configurar API Key globalmente
        global::Stripe.StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public CustomerService CreateCustomerService() => new CustomerService();
    public SubscriptionService CreateSubscriptionService() => new SubscriptionService();
    public PriceService CreatePriceService() => new PriceService();
    public InvoiceService CreateInvoiceService() => new InvoiceService();
    public PaymentIntentService CreatePaymentIntentService() => new PaymentIntentService();
    public BillingPortal.SessionService CreateBillingPortalSessionService() => new BillingPortal.SessionService();
    public Checkout.SessionService CreateCheckoutSessionService() => new Checkout.SessionService();
}