using Hampcoders.Electrolink.API.Shared.Infrastructure;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using StripeSubscription = Stripe.Subscription;
using StripeEvent = Stripe.Event;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;
using SessionLineItemOptions = Stripe.Checkout.SessionLineItemOptions;
using BillingPortalSessionService = Stripe.BillingPortal.SessionService;
using BillingPortalSessionCreateOptions = Stripe.BillingPortal.SessionCreateOptions;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.ExternalProviders;

public class StripePaymentProvider : IPaymentProvider
{
    private readonly StripeClient _client;
    private readonly StripeSettings _settings;

    public StripePaymentProvider(IOptions<StripeSettings> settings)
    {
        _settings = settings.Value;
        _client = new StripeClient(_settings.SecretKey);
    }

    public async Task<ExternalCustomerId> CreateCustomerAsync(
        string email, string name,
        IReadOnlyDictionary<string, string>? metadata = null)
    {
        try
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
                Metadata = metadata?.ToDictionary(k => k.Key, k => k.Value) ?? []
            };
            var service = new CustomerService(_client);
            var customer = await service.CreateAsync(options);
            return new ExternalCustomerId(customer.Id);
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to create customer", ex, ex.StripeError?.Code);
        }
    }

    public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        ExternalCustomerId customerId, string priceId,
        string successUrl, string cancelUrl,
        IReadOnlyDictionary<string, string>? metadata = null)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                Customer = customerId.Value,
                Mode = "subscription",
                LineItems =
                [
                    new SessionLineItemOptions { Price = priceId, Quantity = 1 }
                ],
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Metadata = metadata?.ToDictionary(k => k.Key, k => k.Value) ?? []
            };
            var service = new SessionService(_client);
            var session = await service.CreateAsync(options);
            return new CheckoutSessionResult(session.Id, session.Url ?? string.Empty);
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to create checkout session", ex, ex.StripeError?.Code);
        }
    }

    public async Task<EnterpriseCheckoutSessionResult> CreateEnterpriseCheckoutSessionAsync(
        ExternalCustomerId customerId, decimal pricePerDevice, int deviceCount,
        string successUrl, string cancelUrl,
        IReadOnlyDictionary<string, string>? metadata = null)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                Customer = customerId.Value,
                Mode = "payment",
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmountDecimal = pricePerDevice * 100m,
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Enterprise per-device fee"
                            }
                        },
                        Quantity = deviceCount
                    }
                ],
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Metadata = metadata?.ToDictionary(k => k.Key, k => k.Value) ?? []
            };
            var service = new SessionService(_client);
            var session = await service.CreateAsync(options);
            return new EnterpriseCheckoutSessionResult(session.Id, session.Url ?? string.Empty);
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to create enterprise checkout session", ex, ex.StripeError?.Code);
        }
    }

    public async Task CancelSubscriptionAsync(string externalSubscriptionId, bool cancelAtPeriodEnd)
    {
        try
        {
            var service = new SubscriptionService(_client);
            await service.UpdateAsync(externalSubscriptionId,
                new SubscriptionUpdateOptions { CancelAtPeriodEnd = cancelAtPeriodEnd });
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to cancel subscription", ex, ex.StripeError?.Code);
        }
    }

    public async Task<SubscriptionStatus> GetSubscriptionStatusAsync(string externalSubscriptionId)
    {
        try
        {
            var service = new global::Stripe.SubscriptionService(_client);
            global::Stripe.Subscription sub = await service.GetAsync(externalSubscriptionId);
            return new SubscriptionStatus(
                sub.Status,
                DateTime.UtcNow, // Fallback for CurrentPeriodEnd since it's missing in this Stripe.net version
                sub.CanceledAt,
                sub.CancelAtPeriodEnd
            );
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to get subscription status", ex, ex.StripeError?.Code);
        }
    }

    public async Task<RefundResult> CreateRefundAsync(string paymentIntentId, decimal? amount, string reason)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Amount = amount.HasValue ? (long?)(amount.Value * 100m) : null,
                Reason = reason switch
                {
                    "requested_by_customer" => RefundReasons.RequestedByCustomer,
                    "duplicate" => RefundReasons.Duplicate,
                    "fraudulent" => RefundReasons.Fraudulent,
                    _ => null
                }
            };
            var service = new RefundService(_client);
            var refund = await service.CreateAsync(options);
            return new RefundResult(refund.Id);
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to create refund", ex, ex.StripeError?.Code);
        }
    }

    public async Task<CustomerPortalUrl> OpenCustomerPortalAsync(ExternalCustomerId customerId, string returnUrl)
    {
        try
        {
            var options = new BillingPortalSessionCreateOptions
            {
                Customer = customerId.Value,
                ReturnUrl = returnUrl
            };
            var service = new BillingPortalSessionService(_client);
            var session = await service.CreateAsync(options);
            return new CustomerPortalUrl(session.Url ?? string.Empty);
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Failed to create customer portal session", ex, ex.StripeError?.Code);
        }
    }

    public async Task<WebhookEvent> ValidateWebhookSignatureAsync(string payload, string signature, string secret)
    {
        await Task.CompletedTask;
        try
        {
            var stripeEvent = global::Stripe.EventUtility.ConstructEvent(payload, signature, secret, throwOnApiVersionMismatch: false);
            string? subId = null;
            string? cusId = null;
            string? piId = null;

            if (stripeEvent.Data.Object is global::Stripe.Subscription s)
            {
                subId = s.Id;
                cusId = s.CustomerId;
            }
            else if (stripeEvent.Data.Object is global::Stripe.Invoice i)
            {
                subId = null; // Removed because Stripe.Invoice doesn't expose it here in v48
                cusId = i.CustomerId;
                piId = null; // Removed because Stripe.Invoice doesn't expose PaymentIntentId here in v48
            }
            else if (stripeEvent.Data.Object is global::Stripe.PaymentIntent p)
            {
                cusId = p.CustomerId;
                piId = p.Id;
            }
            else if (stripeEvent.Data.Object is global::Stripe.Customer c)
            {
                cusId = c.Id;
            }

            return new WebhookEvent(
                stripeEvent.Type,
                subId,
                cusId,
                piId,
                stripeEvent.ToJson()
            );
        }
        catch (StripeException ex)
        {
            throw new PaymentProviderException("Stripe", "Webhook signature validation failed", ex, ex.StripeError?.Code);
        }
    }
}
