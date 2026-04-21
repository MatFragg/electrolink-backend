using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

/// <summary>
/// Payment service implementation for the payment service using Stripe.
/// </summary>
public class StripePaymentGatewayService : IPaymentGatewayService
{
    private readonly StripeClientFactory _clientFactory;
    private readonly StripeSettings _config;
    private readonly StripeEventMapper _eventMapper;
    private readonly ILogger<StripePaymentGatewayService> _logger;
    private readonly SessionService _checkoutSessionService;

    public StripePaymentGatewayService(
        StripeClientFactory clientFactory,
        StripeSettings config,
        StripeEventMapper eventMapper,
        ILogger<StripePaymentGatewayService> logger,
        SessionService checkoutSessionService
    )
    {
        _clientFactory = clientFactory;
        _config = config;
        _eventMapper = eventMapper;
        _logger = logger;
        _checkoutSessionService = checkoutSessionService;
    }

    public async Task<PaymentGatewayCustomerId> CreateOrGetCustomerAsync(int userId, string email, string name)
    {
        var customerService = _clientFactory.CreateCustomerService();

        // Buscar si ya existe un customer con este email
        var listOptions = new CustomerListOptions
        {
            Email = email,
            Limit = 1
        };

        var existingCustomers = await customerService.ListAsync(listOptions);
        
        if (existingCustomers.Data.Count > 0)
        {
            var existingCustomer = existingCustomers.Data[0];
            _logger.LogInformation("Customer already exists: {CustomerId} for User {UserId}", existingCustomer.Id, userId);
            return new PaymentGatewayCustomerId(existingCustomer.Id);
        }

        // Create new customer
        var createOptions = new CustomerCreateOptions
        {
            Email = email,
            Name = name,
            Metadata = new Dictionary<string, string>
            {
                { "user_id", userId.ToString() }
            }
        };

        var customer = await customerService.CreateAsync(createOptions);
        _logger.LogInformation("Customer created: {CustomerId} for User {UserId}", customer.Id, userId);;

        return new PaymentGatewayCustomerId(customer.Id);
    }

    public async Task<string> CreateCheckoutSessionAsync(
        PaymentGatewayCustomerId customerId,
        PaymentGatewayPriceId priceId,
        string successUrl,
        string cancelUrl,
        int? trialPeriodDays = null)
    {
        var sessionService = _clientFactory.CreateCheckoutSessionService();

        var options = new SessionCreateOptions
        {
            Customer = customerId.Value,
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId.Value,
                    Quantity = 1
                }
            },
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            AllowPromotionCodes = true, // Allow coupons
            BillingAddressCollection = "auto"
        };

        // Add trial if specified
        if (trialPeriodDays.HasValue && trialPeriodDays.Value > 0)
        {
            options.SubscriptionData = new SessionSubscriptionDataOptions
            {
                TrialPeriodDays = trialPeriodDays.Value
            };
        }

        var session = await sessionService.CreateAsync(options);
        _logger.LogInformation("Checkout session created: {SessionId} for Customer {CustomerId}", session.Id, customerId);

        return session.Url;
    }

    public async Task<SubscriptionInfo?> GetSubscriptionAsync(PaymentGatewaySubscriptionId subscriptionId)
    {
        try
        {
            var subscriptionService = _clientFactory.CreateSubscriptionService();
            var subscription = await subscriptionService.GetAsync(subscriptionId.Value);

            return _eventMapper.MapToSubscriptionInfo(subscription);
        }
        catch (StripeException ex) when (ex.StripeError.Code == "resource_missing")
        {
            _logger.LogWarning("Subscription not found in Stripe: {SubscriptionId}", subscriptionId);
            return null;
        }
    }

    public async Task<DateTime> CancelSubscriptionAtPeriodEndAsync(PaymentGatewaySubscriptionId subscriptionId)
    {
        var subscriptionService = _clientFactory.CreateSubscriptionService();

        var options = new SubscriptionUpdateOptions
        {
            CancelAtPeriodEnd = true
        };

        var subscription = await subscriptionService.UpdateAsync(subscriptionId.Value, options);
        _logger.LogInformation("Subscription {SubscriptionId} canceled at the end of the period: {CancelAt}",
            subscriptionId, subscription.CancelAt);

        return subscription.CancelAt ?? subscription.EndedAt ?? DateTime.MinValue;
    }

    public async Task CancelSubscriptionImmediatelyAsync(PaymentGatewaySubscriptionId subscriptionId)
    {
        var subscriptionService = _clientFactory.CreateSubscriptionService();
        await subscriptionService.CancelAsync(subscriptionId.Value);

        _logger.LogInformation("Subscription {SubscriptionId} canceled immediately", subscriptionId);
    }

    public async Task<SubscriptionInfo> UpdateSubscriptionPlanAsync(
        PaymentGatewaySubscriptionId subscriptionId,
        PaymentGatewayPriceId newPriceId,
        string prorationBehavior = "create_prorations")
    {
        var subscriptionService = _clientFactory.CreateSubscriptionService();

        var currentSubscription = await subscriptionService.GetAsync(subscriptionId.Value);
        var subscriptionItemId = currentSubscription.Items.Data[0].Id;

        var options = new SubscriptionUpdateOptions
        {
            Items = new List<SubscriptionItemOptions>
            {
                new()
                {
                    Id = subscriptionItemId,
                    Price = newPriceId.Value
                }
            },
            ProrationBehavior = prorationBehavior
        };

        var updatedSubscription = await subscriptionService.UpdateAsync(subscriptionId.Value, options);
        _logger.LogInformation("Subscription {SubscriptionId} updated to Price {PriceId}",
            subscriptionId, newPriceId);

        return _eventMapper.MapToSubscriptionInfo(updatedSubscription);
    }

    public async Task<bool> RetryPaymentAsync(string invoiceId)
    {
        try
        {
            var invoiceService = _clientFactory.CreateInvoiceService();
            var invoice = await invoiceService.PayAsync(invoiceId);

            _logger.LogInformation("Retry payment successful for Invoice {InvoiceId}", invoiceId);
            return invoice.Status == "paid";
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Fail to retry payment for Invoice {InvoiceId}", invoiceId);
            return false;
        }
    }

    public async Task<string> CreateBillingPortalSessionAsync(PaymentGatewayCustomerId customerId, string returnUrl)
    {
        var sessionService = _clientFactory.CreateBillingPortalSessionService();

        var options = new global::Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = customerId.Value,
            ReturnUrl = returnUrl
        };

        var session = await sessionService.CreateAsync(options);
        _logger.LogInformation("Billing portal session created for Customer {CustomerId}", customerId);
        return session.Url;
    }
}