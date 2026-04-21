using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

public interface IStripeService
{
    Task<string> CreateCustomerAsync(string userId);

    Task<(string SessionId, string CheckoutUrl)> CreateCheckoutSessionAsync(
        string stripeCustomerId,
        EBusinessRole businessRole,
        EBillingCycle billingCycle,
        string successUrl,
        string cancelUrl);

    Task CancelAtPeriodEndAsync(string stripeSubscriptionId);

    Task<string> CreateCustomerPortalSessionAsync(string stripeCustomerId, string returnUrl);
}

