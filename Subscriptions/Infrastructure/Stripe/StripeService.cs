using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Stripe;
using BillingPortal = Stripe.BillingPortal;
using Checkout = Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

public class StripeService(IConfiguration configuration) : IStripeService
{
    public async Task<string> CreateCustomerAsync(string userId)
    {
        ConfigureApiKey();

        var service = new CustomerService();
        var options = new CustomerCreateOptions
        {
            Metadata = new Dictionary<string, string>
            {
                ["userId"] = userId
            }
        };

        var customer = await service.CreateAsync(options);
        return customer.Id;
    }

    public async Task<(string SessionId, string CheckoutUrl)> CreateCheckoutSessionAsync(
        string stripeCustomerId,
        EBusinessRole businessRole,
        EBillingCycle billingCycle,
        string successUrl,
        string cancelUrl)
    {
        ConfigureApiKey();

        var priceId = ResolvePriceId(businessRole, billingCycle);
        var service = new Checkout.SessionService();

        var options = new Checkout.SessionCreateOptions
        {
            Customer = stripeCustomerId,
            Mode = "subscription",
            LineItems =
            [
                new Checkout.SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1
                }
            ],
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };

        var session = await service.CreateAsync(options);
        return (session.Id, session.Url ?? string.Empty);
    }

    public async Task CancelAtPeriodEndAsync(string stripeSubscriptionId)
    {
        ConfigureApiKey();

        var service = new SubscriptionService();
        await service.UpdateAsync(
            stripeSubscriptionId,
            new SubscriptionUpdateOptions { CancelAtPeriodEnd = true });
    }

    public async Task<string> CreateCustomerPortalSessionAsync(string stripeCustomerId, string returnUrl)
    {
        ConfigureApiKey();

        var service = new BillingPortal.SessionService();
        var options = new BillingPortal.SessionCreateOptions
        {
            Customer = stripeCustomerId,
            ReturnUrl = returnUrl
        };

        var session = await service.CreateAsync(options);
        return session.Url ?? string.Empty;
    }

    private void ConfigureApiKey()
    {
        var secretKey = configuration["Stripe:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Stripe:SecretKey is not configured.");

        StripeConfiguration.ApiKey = secretKey;
    }

    private string ResolvePriceId(EBusinessRole businessRole, EBillingCycle billingCycle)
    {
        var key = (businessRole, billingCycle) switch
        {
            (EBusinessRole.Technician, EBillingCycle.Monthly) => "Stripe:Prices:TechnicianMonthly",
            (EBusinessRole.Technician, EBillingCycle.Annual) => "Stripe:Prices:TechnicianAnnual",
            (EBusinessRole.Homeowner, EBillingCycle.Monthly) => "Stripe:Prices:HomeownerMonthly",
            (EBusinessRole.Homeowner, EBillingCycle.Annual) => "Stripe:Prices:HomeownerAnnual",
            _ => throw new ArgumentException("Invalid role/cycle combination.")
        };

        return configuration[key] ?? throw new InvalidOperationException($"Stripe price is not configured for key '{key}'.");
    }
}
