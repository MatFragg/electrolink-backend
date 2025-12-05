using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

/// <summary>
/// Interface for a service that interacts with the Stripe API.
/// This abstraction prevents the application layer from knowing about Stripe's implementation details.
/// </summary>
public interface IStripeService
{
    Task<Session> CreateSubscriptionCheckoutSession(string priceId, string successUrl, string cancelUrl);
}